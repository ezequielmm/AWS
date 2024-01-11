// <copyright file="InteractiveControlContainerTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows.Controls.Primitives;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Containers.InteractiveControlContainer;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.Containers.InteractiveControlContainer
{
   using InteractiveControlContainer = GUI.VisualElements.Containers.InteractiveControlContainer.InteractiveControlContainer;

   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class InteractiveControlContainerTests
   {
      [Test]
      [Apartment(ApartmentState.STA)]
      public void Constructor_ShouldSetDefaultValues()
      {
         //Arrange
         var container = new InteractiveControlContainer();

         //Act

         //Assert
         Assert.IsNotNull(container);
         Assert.AreEqual(default(bool), container.IsSelected);
         Assert.AreEqual(default(ResizeType), container.ResizeType);
         Assert.AreEqual(default(bool), container.IsDraggable);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void BorderOffsetPositionBeacauseMargins_Calculated_Correctly()
      {
         //Arrange
         var container = new InteractiveControlContainer();

         //Act

         //Assert
         Assert.IsNotNull(container);
         Assert.AreEqual(5, container.OffsetXPositionBeacauseMargins());
         Assert.AreEqual(5, container.OffsetYPositionBeacauseMargins());
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void OutsideBorderPaddingReturnValueTest()
      {
         //Arrange
         var container = new InteractiveControlContainer();

         //Act

         //Assert
         Assert.IsNotNull(container);
         Assert.AreEqual(4, container.OutsideBorderPadding);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void OutsideBorderThicknessReturnValueTest()
      {
         //Arrange
         var container = new InteractiveControlContainer();

         //Act

         //Assert
         Assert.IsNotNull(container);
         Assert.AreEqual(1, container.OutsideBorderThickness);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void RaiseSelectedEvent_ShouldRaiseSelectedEvent()
      {
         //Arrange
         bool raised = false;
         var container = new InteractiveControlContainer();
         container.Selected += (s, e) => raised = true;

         //Act
         container.RaiseSelectedEvent();

         //Assert
         Assert.IsNotNull(container);
         Assert.IsTrue(raised);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void RaiseDragStartedEvent_ShouldRaiseDragStartedEvent()
      {
         //Arrange
         bool raised = false;
         var container = new InteractiveControlContainer();
         container.DragStarted += (s, e) => raised = true;

         //Act
         container.RaiseDragStartedEvent();

         //Assert
         Assert.IsNotNull(container);
         Assert.IsTrue(raised);
      }

      [Test]
      [TestCase(0, 0)]
      [TestCase(10, 20)]
      [TestCase(100, 200)]
      [Apartment(ApartmentState.STA)]
      public void RaiseDragDeltaEvent_ShouldRaiseDragDeltaEvent(int horizontalChange, int verticalChange)
      {
         //Arrange
         DragDeltaEventArgs args = null;

         var container = new InteractiveControlContainer();
         container.DragDelta += (s, e) => args = e;

         //Act
         container.RaiseDragDeltaEvent(horizontalChange, verticalChange);

         //Assert
         Assert.IsNotNull(container);
         Assert.IsNotNull(args);
         Assert.AreEqual(horizontalChange, args.HorizontalChange);
         Assert.AreEqual(verticalChange, args.VerticalChange);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void RaiseDragCompletedEvent_ShouldRaiseDragCompletedEvent()
      {
         //Arrange
         bool raised = false;
         var container = new InteractiveControlContainer();
         container.DragCompleted += (s, e) => raised = true;

         //Act
         container.RaiseDragCompletedEvent();

         //Assert
         Assert.IsNotNull(container);
         Assert.IsTrue(raised);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void RaiseResizeStartedEvent_ShouldRaiseResizeStartedEvent()
      {
         //Arrange
         bool raised = false;
         var container = new InteractiveControlContainer();
         container.ResizeStarted += (s, e) => raised = true;

         //Act
         container.RaiseResizeStartedEvent();

         //Assert
         Assert.IsNotNull(container);
         Assert.IsTrue(raised);
      }

      [Test]
      [TestCase(0, true, 0, true)]
      [TestCase(10, false, 20, false)]
      [TestCase(100, false, 200, true)]
      [Apartment(ApartmentState.STA)]
      public void RaiseResizeDeltaEvent_ShouldRaiseResizeDeltaEvent(
         int horizontalChange,
         bool isFromLeft,
         int verticalChange,
         bool isFromTop)
      {
         //Arrange
         ResizeDeltaEventArgs args = null;

         var container = new InteractiveControlContainer();
         container.ResizeDelta += (s, e) => args = e;

         //Act
         container.RaiseResizeDeltaEvent(horizontalChange, isFromLeft, verticalChange, isFromTop);

         //Assert
         Assert.IsNotNull(container);
         Assert.IsNotNull(args);
         Assert.AreEqual(horizontalChange, args.HorizontalChange);
         Assert.AreEqual(isFromLeft, args.IsFromLeftSide);
         Assert.AreEqual(verticalChange, args.VerticalChange);
         Assert.AreEqual(isFromTop, args.IsFromTopSide);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void RaiseResizeCompletedEvent_ShouldRaiseResizeCompletedEvent()
      {
         //Arrange
         bool raised = false;
         var container = new InteractiveControlContainer();
         container.ResizeCompleted += (s, e) => raised = true;

         //Act
         container.RaiseResizeCompletedEvent();

         //Assert
         Assert.IsNotNull(container);
         Assert.IsTrue(raised);
      }
   }
}
