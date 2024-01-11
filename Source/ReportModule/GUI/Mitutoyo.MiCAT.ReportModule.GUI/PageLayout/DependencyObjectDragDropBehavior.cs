// <copyright file="DependencyObjectDragDropBehavior.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Containers.InteractiveControlContainer;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Telerik.Windows.DragDrop;
using DragEventArgs = Telerik.Windows.DragDrop.DragEventArgs;
using Point = System.Windows.Point;

namespace Mitutoyo.MiCAT.ReportModule.GUI.PageLayout
{
   [ExcludeFromCodeCoverage]
   public class DependencyObjectDragDropBehavior
   {
      public IViewToVMReportComponent ViewToReportComponent { get; set; }
      public DependencyObject DragsTargetContainer { get; set; }
      private Point _relativeStartPoint = new Point(0, 0);

      private IVMDraggablePlacement vmDraggedPlacement = null;
      public IInputElement PositionRelativeForDraggedComponents { get; set; } = null;

      private static Dictionary<DependencyObject, DependencyObjectDragDropBehavior> _behaviorsPerDragsContainers;

      static DependencyObjectDragDropBehavior()
      {
         _behaviorsPerDragsContainers = new Dictionary<DependencyObject, DependencyObjectDragDropBehavior>();
      }

      public static IInputElement GetPositionRelativeForDraggedComponents(DependencyObject obj)
      {
         return (IInputElement)obj.GetValue(PositionRelativeForDraggedComponentsProperty);
      }

      public static void SetPositionRelativeForDraggedComponents(DependencyObject obj, IInputElement value)
      {
         obj.SetValue(PositionRelativeForDraggedComponentsProperty, value);
      }

      // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty PositionRelativeForDraggedComponentsProperty =
         DependencyProperty.RegisterAttached("PositionRelativeForDraggedComponents", typeof(IInputElement), typeof(DependencyObjectDragDropBehavior),
            new PropertyMetadata());

      public static bool GetIsEnabled(DependencyObject obj)
      {
         return (bool)obj.GetValue(IsEnabledProperty);
      }

      public static void SetIsEnabled(DependencyObject obj, bool value)
      {
         if (value)
            AddAndAttachBehavior(obj);
         else
            RemoveAndDettachBehavior(obj);
      }

      private static void RemoveAndDettachBehavior(DependencyObject obj)
      {
         var behavior = GetAttachedBehavior(obj);
         behavior.CleanUp();
         RemoveAttachedBehavior(obj);
      }

      private static void AddAndAttachBehavior(DependencyObject obj)
      {
         var behavior = AddBehaviorToDragsContainer(obj);
         behavior.Initialize();
      }

      private static void RemoveAttachedBehavior(DependencyObject obj)
      {
         _behaviorsPerDragsContainers.Remove(obj);
      }

      // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty IsEnabledProperty =
         DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(DependencyObjectDragDropBehavior),
            new PropertyMetadata(false, new PropertyChangedCallback(OnIsEnabledPropertyChanged)));

      public static void OnIsEnabledPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
      {
         SetIsEnabled(dependencyObject, (bool)e.NewValue);
      }

      private static DependencyObjectDragDropBehavior GetAttachedBehavior(DependencyObject dragsTargetContainer)
      {
         return _behaviorsPerDragsContainers[dragsTargetContainer];
      }

      private static DependencyObjectDragDropBehavior AddBehaviorToDragsContainer(DependencyObject dragsTargetContainer)
      {
         var behaviorForNewDragsTargetContainerRegistered = new DependencyObjectDragDropBehavior();
         _behaviorsPerDragsContainers[dragsTargetContainer] = behaviorForNewDragsTargetContainerRegistered;
         behaviorForNewDragsTargetContainerRegistered.DragsTargetContainer = dragsTargetContainer;
         behaviorForNewDragsTargetContainerRegistered.ViewToReportComponent = new ViewToVMReportComponent();
         behaviorForNewDragsTargetContainerRegistered.PositionRelativeForDraggedComponents =
            (IInputElement)dragsTargetContainer.GetValue(DependencyObjectDragDropBehavior.PositionRelativeForDraggedComponentsProperty);
         return behaviorForNewDragsTargetContainerRegistered;
      }

      protected virtual void Initialize()
      {
         SubscribeToDragDropEvents();
      }
      protected virtual void CleanUp()
      {
         UnsubscribeFromDragDropEvents();
      }

      private void SubscribeToDragDropEvents()
      {
         DragDropManager.AddDragInitializeHandler(this.DragsTargetContainer, OnDragInitialize);
         DragDropManager.AddGiveFeedbackHandler(this.DragsTargetContainer, OnGiveFeedback);
         DragDropManager.AddDragOverHandler(this.DragsTargetContainer, OnDragOver);
         DragDropManager.AddDragDropCompletedHandler(this.DragsTargetContainer, OnDragDropCompleted);
      }
      private void UnsubscribeFromDragDropEvents()
      {
         DragDropManager.RemoveDragInitializeHandler(this.DragsTargetContainer, OnDragInitialize);
         DragDropManager.RemoveGiveFeedbackHandler(this.DragsTargetContainer, OnGiveFeedback);
         DragDropManager.RemoveDragOverHandler(this.DragsTargetContainer, OnDragOver);
         DragDropManager.RemoveDragDropCompletedHandler(this.DragsTargetContainer, OnDragDropCompleted);
      }

      private void OnDragInitialize(object sender, DragInitializeEventArgs e)
      {
         var vmDraggedComponent = ViewToReportComponent.GetVMReportElementFromOriginalSource(e);
         var draggedComponentView = (e.OriginalSource as FrameworkElement)?.TemplatedParent as InteractiveControlContainer;

         if (vmDraggedComponent == null || draggedComponentView == null)
         {
            e.Cancel = true;
            return;
         }
         else
         {
            vmDraggedPlacement = vmDraggedComponent.VMPlacement;

            if (vmDraggedPlacement.IsSelected)
            {
               _relativeStartPoint = new Point(e.RelativeStartPoint.X - draggedComponentView.OffsetXPositionBeacauseMargins(), e.RelativeStartPoint.Y - draggedComponentView.OffsetYPositionBeacauseMargins());
               vmDraggedPlacement.StartDrag();
            }
            else
            {
               e.Cancel = true;
               return;
            }
         }
      }

      private void OnDragDropCompleted(object sender, DragDropCompletedEventArgs e)
      {
         if (vmDraggedPlacement != null)
            vmDraggedPlacement.CompleteDrag();
      }

      private void OnDragOver(object sender, DragEventArgs e)
      {
         if (vmDraggedPlacement == null)
            return;
         else
         {
            IInputElement positionRelativeToUse;

            if (PositionRelativeForDraggedComponents == null)
               positionRelativeToUse = sender as IInputElement;
            else
               positionRelativeToUse = PositionRelativeForDraggedComponents;

            var position = e.GetPosition(positionRelativeToUse);
            var y = (int)(position.Y - _relativeStartPoint.Y);
            var x = (int)(position.X - _relativeStartPoint.X);

            if (vmDraggedPlacement != null && vmDraggedPlacement.IsDragging)
            {
               var horizontalDelta = x - vmDraggedPlacement.VisualX;
               var verticalDelta = y - vmDraggedPlacement.VisualY;

               vmDraggedPlacement.Drag(horizontalDelta, verticalDelta);
            }
         }
      }

      private void OnGiveFeedback(object sender, Telerik.Windows.DragDrop.GiveFeedbackEventArgs e)
      {
         e.SetCursor(Resource.EmbeddedCursors.Cursors.MoveCursor);
         e.Handled = true;
      }
   }
}
