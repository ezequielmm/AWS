// <copyright file="GridViewHeaderMenuTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView;
using Moq;
using NUnit.Framework;
using Telerik.Windows.Controls;
using static Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.InteractiveReportGridView;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.Table.Views.CustomGridView
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class GridViewHeaderMenuTest
   {
      [Test]
      [Apartment(ApartmentState.STA)]
      public void GridViewHeaderMenu_ShouldBeIntializedProperly()
      {
         var gridView = new InteractiveReportGridView();
         var themeMock = new Mock<Theme>();

         var sut = new GridViewHeaderMenu(gridView, themeMock.Object);

         var contextMenu = RadContextMenu.GetContextMenu(gridView);

         Assert.IsNotNull(contextMenu);
         Assert.AreEqual(themeMock.Object, StyleManager.GetTheme(contextMenu));
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void GridViewHeaderMenu_ShouldAttachContextMenuTheme()
      {
         var gridView = new InteractiveReportGridView();
         var firstThemeMock = new Mock<Theme>();
         var secondThemeMock = new Mock<Theme>();

         var sut = new GridViewHeaderMenu(gridView, firstThemeMock.Object);
         sut.Attach(secondThemeMock.Object);

         var contextMenu = RadContextMenu.GetContextMenu(gridView);

         Assert.IsNotNull(contextMenu);
         Assert.AreEqual(secondThemeMock.Object, StyleManager.GetTheme(contextMenu));
      }
   }
}
