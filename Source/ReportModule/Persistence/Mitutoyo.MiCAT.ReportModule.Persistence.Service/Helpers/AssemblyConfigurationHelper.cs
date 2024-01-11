// <copyright file="AssemblyConfigurationHelper.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Configuration;
using System.Linq;
using System.Reflection;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Service.Helpers
{
   public class AssemblyConfigurationHelper : IAssemblyConfigurationHelper
   {
      private readonly Configuration _config;
      public AssemblyConfigurationHelper(Assembly assembly)
      {
         _config = ConfigurationManager.OpenExeConfiguration(assembly.Location);
      }
      public string GetDataServiceApiUrl()
      {
         return GetValueOrDefault("DataServiceApiUrl", "http://localhost:48254/");
      }

      private string GetValueOrDefault(string key, string defaultValue)
      {
         return _config.HasFile && HasKey(key) ? GetValue(key) : defaultValue;
      }
      private bool HasKey(string key)
      {
         return _config.AppSettings.Settings.AllKeys.Contains(key);
      }
      private string GetValue(string key)
      {
         return _config.AppSettings.Settings[key].Value;
      }
   }
}
