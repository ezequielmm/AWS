// <copyright file="PropertyValuesExtensions.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Mitutoyo.MiCAT.Web.Data;

namespace Mitutoyo.MiCAT.ReportModule.Persistence
{
   public static class PropertyValuesExtensions
   {
      public static double? GetSafeDoubleValue(this IEnumerable<PropertyValueDTO> propertyValues, string name)
      {
         var value = propertyValues.SingleOrDefault(p => p.Name == name)?.Value;
         if (value == null) return null;
         if (!double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var number)) return null;
         return number;
      }

      public static string GetSafeStringValue(this IEnumerable<PropertyValueDTO> propertyValues, string name)
      {
         var value = propertyValues.SingleOrDefault(p => p.Name == name)?.Value;
         return value;
      }
   }
}
