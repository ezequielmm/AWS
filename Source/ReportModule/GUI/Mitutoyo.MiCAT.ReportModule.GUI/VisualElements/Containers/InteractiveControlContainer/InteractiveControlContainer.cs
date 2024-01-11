// <copyright file="InteractiveControlContainer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using AdornerLayer = System.Windows.Documents.AdornerLayer;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Containers.InteractiveControlContainer
{
   public partial class InteractiveControlContainer : ContentControl
   {
      public static readonly RoutedEvent SelectedEvent;

      public static readonly RoutedEvent DragStartedEvent;
      public static readonly RoutedEvent DragDeltaEvent;
      public static readonly RoutedEvent DragCompletedEvent;

      public static readonly RoutedEvent ResizeStartedEvent;
      public static readonly RoutedEvent ResizeDeltaEvent;
      public static readonly RoutedEvent ResizeCompletedEvent;

      public static readonly DependencyProperty IsSelectedProperty;
      public static readonly DependencyProperty ResizeTypeProperty;
      public static readonly DependencyProperty IsDraggableProperty;

      static InteractiveControlContainer()
      {
         IsSelectedProperty = DependencyProperty.Register(
            nameof(IsSelected),
            typeof(bool),
            typeof(InteractiveControlContainer),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

         ResizeTypeProperty = DependencyProperty.Register(
            nameof(ResizeType),
            typeof(ResizeType),
            typeof(InteractiveControlContainer),
            new PropertyMetadata(ResizeType.None));

         IsDraggableProperty = DependencyProperty.Register(
            nameof(IsDraggable),
            typeof(bool),
            typeof(InteractiveControlContainer),
            new PropertyMetadata(false));

         SelectedEvent = EventManager.RegisterRoutedEvent(
            nameof(Selected),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(InteractiveControlContainer));

         DragStartedEvent = EventManager.RegisterRoutedEvent(
            nameof(DragStarted),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(InteractiveControlContainer));

         DragDeltaEvent= EventManager.RegisterRoutedEvent(
            nameof(DragDelta),
            RoutingStrategy.Bubble,
            typeof(DragDeltaEventHandler),
            typeof(InteractiveControlContainer));

         DragCompletedEvent = EventManager.RegisterRoutedEvent(
            nameof(DragCompleted),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(InteractiveControlContainer));

         ResizeStartedEvent = EventManager.RegisterRoutedEvent(
            nameof(ResizeStarted),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(InteractiveControlContainer));

         ResizeDeltaEvent = EventManager.RegisterRoutedEvent(
            nameof(ResizeDelta),
            RoutingStrategy.Bubble,
            typeof(ResizeDeltaEventHandler),
            typeof(InteractiveControlContainer));

         ResizeCompletedEvent = EventManager.RegisterRoutedEvent(
            nameof(ResizeCompleted),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(InteractiveControlContainer));
      }

      public bool IsHorizontalResizable =>
         ResizeType == ResizeType.All || ResizeType == ResizeType.Horizontal;

      public bool IsVerticalResizable =>
         ResizeType == ResizeType.All || ResizeType == ResizeType.Vertical;

      public int OutsideBorderPadding
      {
         get => 4;
      }

      public int OutsideBorderThickness
      {
         get => 1;
      }

      public bool IsSelected
      {
         get => (bool)GetValue(IsSelectedProperty);
         set => SetValue(IsSelectedProperty, value);
      }

      public ResizeType ResizeType
      {
         get => (ResizeType)GetValue(ResizeTypeProperty);
         set => SetValue(ResizeTypeProperty, value);
      }

      public bool IsDraggable
      {
         get => (bool)GetValue(IsDraggableProperty);
         set => SetValue(IsDraggableProperty, value);
      }

      public event RoutedEventHandler Selected
      {
         add { AddHandler(SelectedEvent, value); }
         remove { RemoveHandler(SelectedEvent, value); }
      }

      public event RoutedEventHandler DragStarted
      {
         add { AddHandler(DragStartedEvent, value); }
         remove { RemoveHandler(DragStartedEvent, value); }
      }

      public event DragDeltaEventHandler DragDelta
      {
         add { AddHandler(DragDeltaEvent, value); }
         remove { RemoveHandler(DragDeltaEvent, value); }
      }

      public event RoutedEventHandler DragCompleted
      {
         add { AddHandler(DragCompletedEvent, value); }
         remove { RemoveHandler(DragCompletedEvent, value); }
      }

      public event RoutedEventHandler ResizeStarted
      {
         add { AddHandler(ResizeStartedEvent, value); }
         remove { RemoveHandler(ResizeStartedEvent, value); }
      }

      public event ResizeDeltaEventHandler ResizeDelta
      {
         add { AddHandler(ResizeDeltaEvent, value); }
         remove { RemoveHandler(ResizeDeltaEvent, value); }
      }

      public event RoutedEventHandler ResizeCompleted
      {
         add { AddHandler(ResizeCompletedEvent, value); }
         remove { RemoveHandler(ResizeCompletedEvent, value); }
      }

      public int OffsetXPositionBeacauseMargins() => OutsideBorderPadding + OutsideBorderThickness;
      public int OffsetYPositionBeacauseMargins() => OutsideBorderPadding + OutsideBorderThickness;

      public override void OnApplyTemplate()
      {
         base.OnApplyTemplate();

         AddSelectionAdorner();
      }

      public void RaiseSelectedEvent()
      {
         var eventArgs = new RoutedEventArgs(SelectedEvent);
         RaiseEvent(eventArgs);
      }

      public void RaiseDragStartedEvent()
      {
         var eventArgs = new RoutedEventArgs(DragStartedEvent);
         RaiseEvent(eventArgs);
      }

      public void RaiseDragDeltaEvent(int horizontalChange, int verticalChange)
      {
         var eventArgs = new DragDeltaEventArgs(horizontalChange, verticalChange)
         {
            RoutedEvent = DragDeltaEvent,
         };
         RaiseEvent(eventArgs);
      }

      public void RaiseDragCompletedEvent()
      {
         var eventArgs = new RoutedEventArgs(DragCompletedEvent);
         RaiseEvent(eventArgs);
      }

      public void RaiseResizeStartedEvent()
      {
         var eventArgs = new RoutedEventArgs(ResizeStartedEvent);
         RaiseEvent(eventArgs);
      }

      public void RaiseResizeDeltaEvent(int horizontalChange, bool isFromLeftSide, int verticalChange, bool isFromTopSide)
      {
         var eventArgs = new ResizeDeltaEventArgs(horizontalChange, isFromLeftSide, verticalChange, isFromTopSide);
         RaiseEvent(eventArgs);
      }

      public void RaiseResizeCompletedEvent()
      {
         var eventArgs = new RoutedEventArgs(ResizeCompletedEvent);
         RaiseEvent(eventArgs);
      }

      protected override void OnMouseUp(MouseButtonEventArgs e)
      {
         if (e.ChangedButton == MouseButton.Left)
            e.Handled = true; //REFACTOR: Avoid event propagation to report view because of unselect scrollbar issue

         base.OnMouseUp(e);
      }

      protected override Size MeasureOverride(Size constraint)
      {
         if (Content is FrameworkElement element) element.Measure(constraint);

         return base.MeasureOverride(constraint);
      }

      protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
      {
         base.OnPreviewMouseLeftButtonDown(e);
         RaiseSelectedEvent();
      }

      protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
      {
         base.OnGotKeyboardFocus(e);
         RaiseSelectedEvent();
      }

      private void AddSelectionAdorner()
      {
         var adornerLayer = AdornerLayer.GetAdornerLayer(this);
         var selectionAdorner = new SelectionAdorner(this);
         adornerLayer?.Add(selectionAdorner);
      }
   }

   public class ResizeDeltaEventArgs : RoutedEventArgs
   {
      public ResizeDeltaEventArgs(
         int horizontalChange,
         bool isFromLeftSide,
         int verticalChange,
         bool isFromTopSide)
      {
         HorizontalChange = horizontalChange;
         IsFromLeftSide = isFromLeftSide;
         VerticalChange = verticalChange;
         IsFromTopSide = isFromTopSide;
         RoutedEvent = InteractiveControlContainer.ResizeDeltaEvent;
      }

      public int HorizontalChange { get; }
      public bool IsFromLeftSide { get; }
      public int VerticalChange { get; }
      public bool IsFromTopSide { get; }
   }

   public delegate void ResizeDeltaEventHandler(object sender, ResizeDeltaEventArgs e);
}
