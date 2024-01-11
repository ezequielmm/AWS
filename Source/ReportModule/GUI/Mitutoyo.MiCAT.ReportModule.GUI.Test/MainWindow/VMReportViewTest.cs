// <copyright file="VMReportViewTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Windows.Controls;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.ContextMenu;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.MainWindow
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMReportViewTest
   {
      private VMReportView _vmReportView;
      private Mock<IPagesRenderer> _pageRendererMock;
      private Mock<IReportModeProperty> _reportModePropertyMock;
      private Mock<IMainContextMenu> _paletteContextMenu;
      [SetUp]
      public void Setup()
      {
         _pageRendererMock = new Mock<IPagesRenderer>();
         _reportModePropertyMock = new Mock<IReportModeProperty>();
         _paletteContextMenu = new Mock<IMainContextMenu>();
         _vmReportView = new VMReportView(_pageRendererMock.Object, _reportModePropertyMock.Object, _paletteContextMenu.Object);
      }

      [Test]
      public void VMReportView_OnContextMenuOpening_DontSetPositionForNullSource()
      {
         //Arrange
         ContextMenuEventArgs eventArgs = (ContextMenuEventArgs)FormatterServices.GetUninitializedObject(typeof(ContextMenuEventArgs));

         // Act
         _vmReportView.OnContextMenuOpening.Execute(eventArgs);

         // Assert
         _paletteContextMenu.Verify(c => c.SetPagePosition(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
      }
   }
}
