[CmdletBinding()]
param(
    [Parameter(Mandatory = 1)] [string]$projectFile,
    [Parameter(Mandatory = 1)] [string]$nuspecFile
)

Function Get-PackageReferences
{
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = 1, ValueFromPipeline = 1)] [string]$project
    )
    process {
        [xml]$projectXml = Get-Content $project
        $nsmgr = New-Object Xml.XmlNamespaceManager $projectXml.NameTable
        $nsmgr.AddNamespace('ns', $projectXml.DocumentElement.NamespaceURI)
        $projectXml.SelectNodes('//ns:ItemGroup/ns:PackageReference', $nsmgr)
    }
}

Function Get-ProjectReferences
{
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = 1, ValueFromPipeline = 1)] [string]$project
    )
    process {
        [xml]$projectXml = Get-Content $project
        $nsmgr = New-Object Xml.XmlNamespaceManager $projectXml.NameTable
        $nsmgr.AddNamespace('ns', $projectXml.DocumentElement.NamespaceURI)
        $projectXml.SelectNodes('//ns:ItemGroup/ns:ProjectReference', $nsmgr)
    }
}

Function Get-Projects
{
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = 1, ValueFromPipeline = 1)] [string]$project
    )
    process {
        $projectPath = (Resolve-Path -LiteralPath $project).Path

        $stack = New-Object System.Collections.Generic.Stack[string]
        $stack.Push($projectPath)
        $set = @{ $projectPath = $true }

        while ($stack.Count -gt 0)
        {
            $currentProject = $stack.Pop()

            $currentProject

            $currentDirectory = Split-Path -Path $currentProject -Parent
            $projects = Get-ProjectReferences $currentProject |
                        ForEach-Object { (Resolve-Path -LiteralPath (Join-Path $currentDirectory $_.Include)).Path }

            foreach ($p in $projects)
            {
                if ($set.Contains($p))
                {
                    continue
                }

                $stack.Push($p)
                $set.Add($p, $true)
            }
        }
    }
}

try
{
    $packageReferences = Get-Projects $projectFile | Get-PackageReferences |
                         Where-Object { $_.PrivateAssets -ne 'all' } | Sort-Object -Unique -Property Include,Version
    
    [xml]$nuspecXml = Get-Content $nuspecFile -Encoding utf8
    $xmlns = $nuspecXml.DocumentElement.NamespaceURI

    $group = $nuspecXml.package.metadata.dependencies.group | Select-Object -First 1
    $oldDependencies = $group.dependency | ForEach-Object { $_.ParentNode.RemoveChild($_) }

    foreach ($package in $packageReferences)
    {
        if ($group.dependency | Where-Object { $_.id -eq $package.Include })
        {
            Write-Error "Different versions for package: $($package.Include)" -ErrorAction Stop
        }

        $oldDependency = $oldDependencies | Where-Object { $_.id -eq $package.Include }
        if ($oldDependency -is [array])
        {
            Write-Error "Duplicate dependency in nuspec for package: $($package.Include)" -ErrorAction Stop
        }

        if ($oldDependency)
        {
            $oldDependency.SetAttribute('version', $package.Version)
            $null = $group.AppendChild($oldDependency)
        }
        else
        {
            $dependency = $nuspecXml.createnode('element', 'dependency', $xmlns)
            $dependency.SetAttribute('id', $package.Include)
            $dependency.SetAttribute('version', $package.Version)
            $dependency.SetAttribute('exclude', 'Build,Analyzers')
            $null = $group.AppendChild($dependency)
        }
    }

    $nuspecXml.Save($nuspecFile)
    Write-Output "Updated nuspec: $nuspecFile"
}
catch
{
    $Error
    Write-Error "Update nuspec failed: $nuspecFile"
    exit 1
}
