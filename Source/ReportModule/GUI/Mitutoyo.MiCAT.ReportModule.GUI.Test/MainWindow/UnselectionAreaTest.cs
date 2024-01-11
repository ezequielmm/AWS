// <copyright file="UnselectionAreaTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.Views;
using Moq;
using NUnit.Framework;
using Telerik.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.MainWindow
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class UnselectionAreaTest
   {
      [Test]
      [Apartment(ApartmentState.STA)]
      public void CheckIsSelectedShouldReturnTrueOnReportBoundarySection()
      {
         //Arrange
         var originalSource = new Canvas() {  Name = "ReportBoundarySectionComponentsContainer" };
         var source = new PagesView();

         TestCheckIsSelected(originalSource, source, true);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void CheckIsSelectedShouldReturnTrueOnPageCanvasContainer()
      {
         //Arrange
         var originalSource = new Canvas { Name = "PageCanvasContainer" };
         var source = new PagesView();

         TestCheckIsSelected(originalSource, source, true);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void CheckIsSelectedShouldReturnTrueOnGrayedArea()
      {
         var originalSource = new Border();
         var source = new CarouselScrollViewer();

         TestCheckIsSelected(originalSource, source, true);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void CheckIsSelectedShouldReturnFalseOnSelectionEllipse()
      {
         var originalSource = new Ellipse();
         var source = new CarouselScrollViewer();

         TestCheckIsSelected(originalSource, source, false);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void CheckIsSelectedShouldReturnTrueBetweenPages()
      {
         var vmPagesMock = new Mock<IVMPages>();
         var originalSource = new Border { DataContext = vmPagesMock.Object };
         var source = new PagesView();

         TestCheckIsSelected(originalSource, source, true);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void CheckIsSelectedShouldReturnFalseBetweenPages()
      {
         var originalSource = new Border { DataContext = new HeaderFormField() };
         var source = new PagesView();

         TestCheckIsSelected(originalSource, source, false);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void CheckIsSelectedShouldReturnFalseOnReportComponent()
      {
         var originalSource = new Canvas();
         var source = new HeaderForm();

         TestCheckIsSelected(originalSource, source, false);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void CheckIsSelectedShouldReturnFalseOnReportComponentOnPageCanvasContainer()
      {
         var originalSource = new Canvas { Name = "PageCanvasContainer" };
         var source = new HeaderForm();

         TestCheckIsSelected(originalSource, source, false);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void CheckIsSelectedShouldReturnFalseOnReportComponentOnReportBoundarySectionComponentsContainer()
      {
         var originalSource = new ReportBoundarySectionView();
         var source = new HeaderForm();

         TestCheckIsSelected(originalSource, source, false);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void CheckIsSelectedShouldReturnTrueOnThumb()
      {
         var originalSource = new Rectangle();

         SetTemplatedParent(originalSource, new Thumb());
         SetTemplatedParent(originalSource.TemplatedParent as Thumb, null);

         object source = new CarouselScrollViewer();

         TestCheckIsSelected(originalSource, source, true);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void CheckIsSelectedShouldReturnFalseOnScrollbar()
      {
         var originalSource = new Rectangle();

         SetTemplatedParent(originalSource, new Thumb());
         SetTemplatedParent(originalSource.TemplatedParent as Thumb, new ScrollBar());

         object source = null;

         TestCheckIsSelected(originalSource, source, false);
      }

      private void TestCheckIsSelected(object originalSource, object source, bool expectedResult)
      {
         //Arrange
         var sut = new UnselectionArea();

         //Act
         var result = sut.CheckIsSelected(originalSource, source);

         //Assert
         Assert.AreEqual(expectedResult, result);
      }

      private void SetTemplatedParent(FrameworkElement frameworkElement, object value)
      {
         var _templatedParent = frameworkElement.GetType().GetField("_templatedParent", BindingFlags.Instance | BindingFlags.NonPublic);
         _templatedParent.SetValue(frameworkElement, value);
      }
   }
}
