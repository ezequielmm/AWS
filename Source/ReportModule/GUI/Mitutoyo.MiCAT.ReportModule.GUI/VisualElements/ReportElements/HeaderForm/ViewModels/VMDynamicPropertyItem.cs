// <copyright file="VMDynamicPropertyItem.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels
{
   public class VMDynamicPropertyItem
   {
      public static VMDynamicPropertyItem EmptyDynamicPropertyItem => new VMDynamicPropertyItem
      {
         Id = Guid.Empty,
         Name = "SelectProperty",
         DisplayName = LocalizeDynamicPropertyItemName("SelectProperty"),
         EntityType = EntityType.None,
      };

      public static string LocalizeDynamicPropertyItemName(string value) =>
         Resources.ResourceManager.GetString($"FieldItem_{value}") ?? value;

      public Guid Id { get; set; }
      public string Name { get; set; }
      public string DisplayName { get; set; }
      public EntityType EntityType { get; set; }
   }
}
