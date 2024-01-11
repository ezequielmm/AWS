// <copyright file="VMPageSizeSettingsItemTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Common.Filebar
{
   [ExcludeFromCodeCoverage]
   public class VMPageSizeSettingsItemTest
   {
      [Test]
      public void Init_ShouldLocalizeDisplayName()
      {
         // Arrange
         var pageSizeInfo = new PageSizeInfo(PaperKind.A4, 100, 200);

         // Act
         var vm = new VMPageSizeSettingsItem(pageSizeInfo);

         // Assert
         Assert.AreEqual(pageSizeInfo, vm.PageSizeInfo);
         Assert.IsNotNull(vm.DisplayName);
         Assert.AreEqual(Properties.Resources.PageSizeA4, vm.DisplayName);
      }
   }
}
