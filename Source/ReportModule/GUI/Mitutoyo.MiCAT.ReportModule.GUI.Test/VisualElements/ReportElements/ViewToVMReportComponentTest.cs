// <copyright file="ViewToVMReportComponentTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ViewToVMReportComponentTest
   {
      private IViewToVMReportComponent _viewToComponent;

      [SetUp]
      public void Setup()
      {
         _viewToComponent = new ViewToVMReportComponent();
      }

      #region "Getting VM from frameworkElement"
      [Test]
      [Apartment(ApartmentState.STA)]
      public void GetVMReportComponentTest()
      {
         //Arrange
         var viewElement = new FrameworkElement();
         var vmReportComponent = (new Mock<IVMReportComponent>()).Object;

         viewElement.DataContext = vmReportComponent;

         //Act
         var vmResult = _viewToComponent.GetVMReportElement(viewElement);

         //Assert
         Assert.AreEqual(vmReportComponent, vmResult);
      }
      [Test]
      [Apartment(ApartmentState.STA)]
      public void GetVMReportComponentWithNoParentTest()
      {
         //Arrange
         var viewElement = new FrameworkElement();

         viewElement.DataContext = new Object();

         //Act
         var vmResult = _viewToComponent.GetVMReportElement(viewElement);

         //Assert
         Assert.IsNull(vmResult);
      }
      [Test]
      [Apartment(ApartmentState.STA)]
      public void GetVMReportComponentForNullInputTest()
      {
         //Arrange

         //Act
         object vmResult = _viewToComponent.GetVMReportElement(null);

         //Assert
         Assert.IsNull(vmResult);
      }
      [Test]
      [Apartment(ApartmentState.STA)]
      public void GetVMReportComponentFromParentTest()
      {
         //Arrange
         var parentElement = new FrameworkElementToTest();
         var childElement = parentElement.Child;
         var vmReportComponent = (new Mock<IVMReportComponent>()).Object;

         parentElement.DataContext = vmReportComponent;

         //Act
         var vmResult = _viewToComponent.GetVMReportElement(childElement);

         //Assert
         Assert.AreEqual(vmReportComponent, vmResult);
      }
      #endregion

      #region "Validating Is VMReportComponent"
      [Test]
      [Apartment(ApartmentState.STA)]
      public void IsViewOfReportComponentTest()
      {
         //Arrange
         var viewElement = new FrameworkElement();
         var vmReportComponent = (new Mock<IVMReportComponent>()).Object;

         viewElement.DataContext = vmReportComponent;

         //Act
         var result = _viewToComponent.IsViewOfReportElement(viewElement);

         //Assert
         Assert.IsTrue(result);
      }
      [Test]
      [Apartment(ApartmentState.STA)]
      public void IsViewOfReportComponentWithNoParentTest()
      {
         //Arrange
         var viewElement = new FrameworkElement();

         viewElement.DataContext = new Object();

         //Act
         var result = _viewToComponent.IsViewOfReportElement(viewElement);

         //Assert
         Assert.IsFalse(result);
      }
      [Test]
      [Apartment(ApartmentState.STA)]
      public void IsViewOfReportComponentForNullInputTest()
      {
         //Arrange

         //Act
         var result = _viewToComponent.IsViewOfReportElement(null);

         //Assert
         Assert.IsFalse(result);
      }
      [Test]
      [Apartment(ApartmentState.STA)]
      public void IsViewOfReportComponentFromParentTest()
      {
         //Arrange
         var parentElement = new FrameworkElementToTest();
         var childElement = parentElement.Child;
         var vmReportComponent = (new Mock<IVMReportComponent>()).Object;

         parentElement.DataContext = vmReportComponent;

         //Act
         var result = _viewToComponent.IsViewOfReportElement(childElement);

         //Assert
         Assert.IsTrue(result);
      }
      #endregion

      [Test]
      public void GetVMReportComponentFromOriginalSourceNullParameterTest()
      {
         //Arrange

         //Act
         var vmResult = _viewToComponent.GetVMReportElementFromOriginalSource(null);

         //Assert
         Assert.IsNull(vmResult);
      }
      [Test]
      public void IsOriginalSourceViewOfReportComponentForNullParameterTest()
      {
         //Arrange

         //Act
         var result = _viewToComponent.IsOriginalSourceViewOfReportElement(null);

         //Assert
         Assert.IsFalse(result);
      }

      private class FrameworkElementToTest : UserControl
      {
         public FrameworkElementToTest()
         {
            Child = new Grid();
            Child.DataContext = new Object();
            Content = Child;
         }
         public FrameworkElement Child { get; }
      }
   }
}
