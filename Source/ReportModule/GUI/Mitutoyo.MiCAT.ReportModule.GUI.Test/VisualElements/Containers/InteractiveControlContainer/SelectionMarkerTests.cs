// <copyright file="SelectionMarkerTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Threading;
using System.Windows.Controls.Primitives;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Containers.InteractiveControlContainer;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.Containers.InteractiveControlContainer
{
   using InteractiveControlContainer = GUI.VisualElements.Containers.InteractiveControlContainer.InteractiveControlContainer;

   public class SelectionMarkerTests
   {
      private IVMReportComponentPlacement _placement;
      private InteractiveControlContainer _container;

      [SetUp]
      public void SetUp()
      {
         _placement = Mock.Of<IVMReportComponentPlacement>();
         _container = new InteractiveControlContainer();

         _container.DragStarted += (s, e) => _placement.StartDrag();
         _container.DragDelta += (s, e) => _placement.Drag(It.IsAny<int>(), It.IsAny<int>());
         _container.DragCompleted += (s, e) => _placement.CompleteDrag();

         _container.ResizeStarted += (s, e) => _placement.StartResize();
         _container.ResizeDelta += (s, e) => _placement.Resize(
            It.IsAny<int>(),
            It.IsAny<bool>(),
            It.IsAny<int>(),
            It.IsAny<bool>());
         _container.ResizeCompleted += (s, e) => _placement.CompleteResize();
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void SelectionMarker_ShouldInitializeControl()
      {
         var marker = new SelectionMarker(_container, MarkerType.TopLeft);

         Assert.IsNotNull(marker.Control);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void OnMarkerDragStarted_OverDraggableComponent_ShouldRaiseDragStartedEvent()
      {
         _container.ResizeType = ResizeType.None;
         _container.IsDraggable = true;

         var marker = new SelectionMarker(_container, MarkerType.TopLeft);

         marker.RaiseEvent(new DragStartedEventArgs(100, 100));

         Mock
            .Get(_placement)
            .Verify(x => x.StartDrag(), Times.Once);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void OnMarkerDragDelta_OverDraggableComponent_ShouldRaiseDragDeltaEvent()
      {
         _container.ResizeType = ResizeType.None;
         _container.IsDraggable = true;

         var marker = new SelectionMarker(_container, MarkerType.TopLeft);

         marker.RaiseEvent(new DragDeltaEventArgs(100, 100));

         Mock
            .Get(_placement)
            .Verify(x => x.Drag(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void OnMarkerDragCompleted_OverDraggableComponent_ShouldRaiseDragCompletedEvent()
      {
         _container.ResizeType = ResizeType.None;
         _container.IsDraggable = true;

         var marker = new SelectionMarker(_container, MarkerType.TopLeft);

         marker.RaiseEvent(new DragCompletedEventArgs(100, 100, false));

         Mock
            .Get(_placement)
            .Verify(x => x.CompleteDrag(), Times.Once);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void OnMarkerDragStarted_OverResizableComponent_ShouldRaiseResizeStartedEvent()
      {
         _container.ResizeType = ResizeType.All;

         var marker = new SelectionMarker(_container, MarkerType.TopLeft);

         marker.RaiseEvent(new DragStartedEventArgs(100, 100));

         Mock
            .Get(_placement)
            .Verify(x => x.StartResize(), Times.Once);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void OnMarkerDragDelta_OverResizableComponent_ShouldRaiseResizeDeltaEvent()
      {
         _container.ResizeType = ResizeType.All;

         var marker = new SelectionMarker(_container, MarkerType.TopLeft);

         marker.RaiseEvent(new DragDeltaEventArgs(100, 100));

         Mock
            .Get(_placement)
            .Verify(
               x => x.Resize(
                  It.IsAny<int>(),
                  It.IsAny<bool>(),
                  It.IsAny<int>(),
                  It.IsAny<bool>()),
               Times.Once);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void OnMarkerDragCompleted_OverResizableComponent_ShouldRaiseResizeCompletedEvent()
      {
         _container.ResizeType = ResizeType.All;

         var marker = new SelectionMarker(_container, MarkerType.TopLeft);

         marker.RaiseEvent(new DragCompletedEventArgs(100, 100, false));

         Mock
            .Get(_placement)
            .Verify(x => x.CompleteResize(), Times.Once);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void OnMarkerDragStarted_ResizeShouldPrecedeDrag()
      {
         _container.ResizeType = ResizeType.All;
         _container.IsDraggable = true;

         var marker = new SelectionMarker(_container, MarkerType.TopLeft);

         marker.RaiseEvent(new DragStartedEventArgs(100, 100));

         Mock
            .Get(_placement)
            .Verify(x => x.StartResize(), Times.Once);

         Mock
            .Get(_placement)
            .Verify(x => x.StartDrag(), Times.Never);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void OnMarkerDragDelta_ResizeShouldPrecedeDrag()
      {
         _container.ResizeType = ResizeType.All;
         _container.IsDraggable = true;

         var marker = new SelectionMarker(_container, MarkerType.TopLeft);

         marker.RaiseEvent(new DragDeltaEventArgs(100, 100));

         Mock
            .Get(_placement)
            .Verify(x =>
               x.Resize(
                  It.IsAny<int>(),
                  It.IsAny<bool>(),
                  It.IsAny<int>(),
                  It.IsAny<bool>()),
               Times.Once);

         Mock
            .Get(_placement)
            .Verify(x => x.StartDrag(), Times.Never);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void OnMarkerDragCompleted_ResizeShouldPrecedeDrag()
      {
         _container.ResizeType = ResizeType.All;
         _container.IsDraggable = true;

         var marker = new SelectionMarker(_container, MarkerType.TopLeft);

         marker.RaiseEvent(new DragCompletedEventArgs(100, 100, false));

         Mock
            .Get(_placement)
            .Verify(x => x.CompleteResize(), Times.Once);

         Mock
            .Get(_placement)
            .Verify(x => x.CompleteDrag(), Times.Never);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void OnMarkerDragStarted_OverNotResizableAndNotDragable_ShouldDoNothing()
      {
         _container.ResizeType = ResizeType.None;
         _container.IsDraggable = false;

         var marker = new SelectionMarker(_container, MarkerType.TopLeft);

         marker.RaiseEvent(new DragStartedEventArgs(100, 100));

         Mock
            .Get(_placement)
            .Verify(x => x.StartResize(), Times.Never);

         Mock
            .Get(_placement)
            .Verify(x => x.StartDrag(), Times.Never);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void OnMarkerDragDelta_OverNotResizableAndNotDragable_ShouldDoNothing()
      {
         _container.ResizeType = ResizeType.None;
         _container.IsDraggable = false;

         var marker = new SelectionMarker(_container, MarkerType.TopLeft);

         marker.RaiseEvent(new DragDeltaEventArgs(100, 100));

         Mock
            .Get(_placement)
            .Verify(x =>
               x.Resize(
                  It.IsAny<int>(),
                  It.IsAny<bool>(),
                  It.IsAny<int>(),
                  It.IsAny<bool>()),
               Times.Never);

         Mock
            .Get(_placement)
            .Verify(x => x.Drag(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void OnMarkerDragCompleted_OverNotResizableAndNotDragable_ShouldDoNothing()
      {
         _container.ResizeType = ResizeType.None;
         _container.IsDraggable = false;

         var marker = new SelectionMarker(_container, MarkerType.TopLeft);

         marker.RaiseEvent(new DragCompletedEventArgs(100, 100, false));

         Mock
            .Get(_placement)
            .Verify(x => x.CompleteResize(), Times.Never);

         Mock
            .Get(_placement)
            .Verify(x => x.CompleteDrag(), Times.Never);
      }
   }
}
