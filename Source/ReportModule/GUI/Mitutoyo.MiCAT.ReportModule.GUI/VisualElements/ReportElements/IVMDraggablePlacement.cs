// <copyright file="IVMDraggablePlacement.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.Common.GUI.Commands;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements
{
   public interface IVMDraggablePlacement : IVMSelectablePlacement
   {
      bool IsDraggable { get; }
      bool IsDragging { get; }
      void StartDrag();
      void Drag(int horizontalDelta, int verticalDelta);
      RelayCommand<DragCommandArgs> DragCommand { get; }
      RelayCommand StartDragCommand { get; }
      RelayCommand CompleteDragCommand { get; }
      void CompleteDrag();
   }

   public struct DragCommandArgs
   {
      public int HorizontalDelta;
      public int VerticalDelta;
   }
}
