// <copyright file="VMDraggablePlacement.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.Common.GUI.Commands;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements
{
   public abstract class VMDraggablePlacement : VMSelectablePlacement, IVMDraggablePlacement
   {
      private bool _isDragging;

      public VMDraggablePlacement()
      {
         StartDragCommand = new RelayCommand(x => StartDrag());
         DragCommand = new RelayCommand<DragCommandArgs>(x => Drag(x.HorizontalDelta, x.VerticalDelta));
         CompleteDragCommand = new RelayCommand(x => CompleteDrag());
      }

      public RelayCommand<DragCommandArgs> DragCommand { get; }

      public RelayCommand StartDragCommand { get; }

      public RelayCommand CompleteDragCommand { get; }

      public bool IsDraggable => true;

      public bool IsDragging
      {
         get => _isDragging;
         private set
         {
            _isDragging = value;
            RaisePropertyChanged();
         }
      }
      public void StartDrag()
      {
         if (IsDragging) return;

         IsDragging = true;
      }

      public void Drag(int horizontalDelta, int verticalDelta)
      {
         if (!IsDragging) return;

         VisualX += horizontalDelta;
         VisualY += verticalDelta;
      }

      public void CompleteDrag()
      {
         if (!IsDragging) return;

         IsDragging = false;
         OnCompleteDrag();
      }
      protected abstract void OnCompleteDrag();
   }
}
