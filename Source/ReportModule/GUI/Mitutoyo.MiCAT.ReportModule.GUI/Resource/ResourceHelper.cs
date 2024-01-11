// <copyright file="ResourceHelper.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Resource
{
   public static class ResourceHelper
   {
      public static Stream GetEmbeddedResourceStream(string filePath)
      {
         var assembly = Assembly.GetExecutingAssembly();
         var assemblyName = assembly.GetName().Name;
         var path = $"{assemblyName}.{filePath}";
         var resourceExists = assembly
            .GetManifestResourceNames()
            .Any(x =>
               string.Equals(x, path, System.StringComparison.InvariantCultureIgnoreCase));
         if (!resourceExists) throw new ArgumentException($"Resource {path} not found as embedded resource.");

         return assembly.GetManifestResourceStream(path);
      }
   }
}
