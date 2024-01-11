// <copyright file="DataServiceLocalizationFinder.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.GUI.Strings
{
   public class DataServiceLocalizationFinder
    {
      static public string FindLocalizedString(string key)
      {
         return StringFinder.FindLocalizedString(key);
      }

      public static string FindCharacteristicTypeName(string key)
      {
         return StringFinder.FindLocalizedString($"CharacteristicType_{key}", key);
      }

      public static string FindCharacteristicDetailValue(string key)
      {
         return StringFinder.FindLocalizedString($"CharacteristicDetail_{key}", key ?? string.Empty);
      }
   }
}
