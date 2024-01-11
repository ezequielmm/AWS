// <copyright file="CodeGenerationAssemblyInitializer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using Mitutoyo.MiCAT.Core.StdLib.Initialization;
using Mitutoyo.MiCAT.Migration.CodeGeneration;
using Mitutoyo.MiCAT.ReportModule.Persistence.MigrationCode;

[assembly: AssemblyInitializer(typeof(CodeGenerationAssemblyInitializer))]

namespace Mitutoyo.MiCAT.ReportModule.Persistence.MigrationCode
{
   internal class CodeGenerationAssemblyInitializer : ICodeGenerationAssemblyInitializer
   {
      public IEnumerable<ITypeInfoFactory> GetTypeInfoFactories(ITypeInfoContext context, IAssemblyInfo assemblyInfo)
      {
         yield return new TypeInfoFactory(context, assemblyInfo);
      }
   }
}