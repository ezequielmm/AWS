// <copyright file="StringFinder.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Globalization;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Strings
{
   public static class StringFinder
   {
      static public string FindLocalizedString(string key)
      {
         return FindLocalizedString(key, string.Empty);
      }

      public static string FindLocalizedString(string key, string defaultValue)
      {
         var resourceSet = Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);

         return resourceSet.GetString(key) ?? defaultValue;
      }
   }
}
