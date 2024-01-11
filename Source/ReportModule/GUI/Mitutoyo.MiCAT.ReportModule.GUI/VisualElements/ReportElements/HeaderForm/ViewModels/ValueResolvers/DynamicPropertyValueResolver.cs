// <copyright file="DynamicPropertyValueResolver.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels.ValueResolvers
{
   public class DynamicPropertyValueResolver : ValueResolver
   {
      private Func<IEnumerable<DynamicPropertyValue>> _getDynamicPropertyValuesFunc;
      protected readonly VMDynamicPropertyItem _dynamicProperty;

      public DynamicPropertyValueResolver(
         Func<IEnumerable<DynamicPropertyValue>> getDynamicPropertyValuesFunc,
         VMDynamicPropertyItem dynamicProperty)
      {
         _getDynamicPropertyValuesFunc = getDynamicPropertyValuesFunc;
         _dynamicProperty = dynamicProperty;
      }

      public override string GetValue() => DynamicPropertyValue?.Value ?? string.Empty;

      private DynamicPropertyValue DynamicPropertyValue =>
         _getDynamicPropertyValuesFunc()?.FirstOrDefault(x => x.Name == _dynamicProperty.Name);
   }
}
