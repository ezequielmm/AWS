// <copyright file="SelectionMarker.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using System.Windows.Controls.Primitives;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Containers.InteractiveControlContainer
{
   public class SelectionMarker : Thumb
   {
      public static readonly DependencyProperty MarkerTypeProperty;

      static SelectionMarker()
      {
         MarkerTypeProperty = DependencyProperty.Register(
            nameof(MarkerType),
            typeof(MarkerType),
            typeof(SelectionMarker),
            new PropertyMetadata());
      }

      public SelectionMarker(InteractiveControlContainer control, MarkerType markerType)
      {
         Control = control;
         MarkerType = markerType;

         Initialize();
      }

      public InteractiveControlContainer Control { get; }

      private bool IsFromLeftSide =>
         MarkerType == MarkerType.Left ||
         MarkerType == MarkerType.TopLeft ||
         MarkerType == MarkerType.BottomLeft;

      private bool IsFromRightSide =>
         MarkerType == MarkerType.Right ||
         MarkerType == MarkerType.TopRight ||
         MarkerType == MarkerType.BottomRight;

      private bool IsFromTopSide =>
         MarkerType == MarkerType.Top ||
         MarkerType == MarkerType.TopLeft ||
         MarkerType == MarkerType.TopRight;

      private bool IsFromBottomSide =>
         MarkerType == MarkerType.Bottom ||
         MarkerType == MarkerType.BottomLeft ||
         MarkerType == MarkerType.BottomRight;

      private bool CanResize
      {
         get
         {
            {
               if (Control.ResizeType == ResizeType.All)
                  return true;

               if (Control.IsHorizontalResizable)
                  return IsFromRightSide || IsFromLeftSide;

               if (Control.IsVerticalResizable)
                  return IsFromTopSide || IsFromBottomSide;

               return false;
            }
         }
      }

      private bool CanDrag
      {
         get
         {
            return Control.IsDraggable;
         }
      }

      public MarkerType MarkerType
      {
         get => (MarkerType)GetValue(MarkerTypeProperty);
         private set => SetValue(MarkerTypeProperty, value);
      }

      public void ArrangeIn(FrameworkElement innerElement)
      {
         var left = CalculateLeftPositionCoordinate(innerElement);
         var top = CalculateTopPositionCoordinate(innerElement);

         var position = new Point(left, top);
         var size = new Size(Width, Height);

         var rect = new Rect(position, size);

         Arrange(rect);
      }

      private void Initialize()
      {
         DragStarted += OnDragStarted;
         DragDelta += OnDragDelta;
         DragCompleted += OnDragCompleted;
      }

      private void OnDragStarted(object sender, DragStartedEventArgs args)
      {
         if (CanResize)
            Control.RaiseResizeStartedEvent();
         else if (CanDrag)
            Control.RaiseDragStartedEvent();
      }

      private void OnDragDelta(object sender, DragDeltaEventArgs args)
      {
         if (CanResize)
         {
            var horizontalDelta = CalculateHorizontalDeltaForResize(args);
            var verticalDelta = CalculateVerticalDeltaForResize(args);

            Control.RaiseResizeDeltaEvent(horizontalDelta, IsFromLeftSide, verticalDelta, IsFromTopSide);
         }
         else if (CanDrag)
            Control.RaiseDragDeltaEvent((int)args.HorizontalChange, (int)args.VerticalChange);
      }

      private void OnDragCompleted(object sender, DragCompletedEventArgs args)
      {
         if (CanResize)
            Control.RaiseResizeCompletedEvent();
         else if (CanDrag)
            Control.RaiseDragCompletedEvent();
      }

      private int CalculateHorizontalDeltaForResize(DragDeltaEventArgs args)
      {
         switch (MarkerType)
         {
            case MarkerType.Left:
            case MarkerType.TopLeft:
            case MarkerType.BottomLeft:
               return -(int)args.HorizontalChange;

            case MarkerType.Right:
            case MarkerType.TopRight:
            case MarkerType.BottomRight:
               return (int)args.HorizontalChange;

            default:
               return 0;
         }
      }

      private int CalculateVerticalDeltaForResize(DragDeltaEventArgs args)
      {
         switch (MarkerType)
         {
            case MarkerType.Top:
            case MarkerType.TopLeft:
            case MarkerType.TopRight:
               return -(int)args.VerticalChange;

            case MarkerType.Bottom:
            case MarkerType.BottomLeft:
            case MarkerType.BottomRight:
               return (int)args.VerticalChange;

            default:
               return 0;
         }
      }

      private int CalculateLeftPositionCoordinate(FrameworkElement innerElement)
      {
         var width = (int)ActualWidth;
         var containerWidth = (int)innerElement.ActualWidth;

         switch (MarkerType)
         {
            case MarkerType.Left:
            case MarkerType.TopLeft:
            case MarkerType.BottomLeft:
               return -width / 2;

            case MarkerType.Right:
            case MarkerType.TopRight:
            case MarkerType.BottomRight:
               return containerWidth - width / 2;

            default:
               return (containerWidth - width) / 2;
         }
      }

      private int CalculateTopPositionCoordinate(FrameworkElement innerElement)
      {
         var height = (int)ActualHeight;
         var containerHeight = (int)innerElement.ActualHeight;

         switch (MarkerType)
         {
            case MarkerType.Top:
            case MarkerType.TopLeft:
            case MarkerType.TopRight:
               return -height / 2;

            case MarkerType.Bottom:
            case MarkerType.BottomLeft:
            case MarkerType.BottomRight:
               return containerHeight - height / 2;

            default:
               return (containerHeight - height) / 2;
         }
      }
   }

   public enum MarkerType
   {
      TopLeft,
      Left,
      BottomLeft,
      Bottom,
      BottomRight,
      Right,
      TopRight,
      Top,
   }
}
