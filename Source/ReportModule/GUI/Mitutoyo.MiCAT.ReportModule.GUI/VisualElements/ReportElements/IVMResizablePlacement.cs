// <copyright file="IVMResizablePlacement.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows.Input;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements
{
   public interface IVMResizablePlacement : IVMDraggablePlacement
   {
      ResizeType ResizeType { get; set; }
      bool IsResizable { get; }
      bool IsHorizontalResizable { get; }
      bool IsVerticalResizable { get; }
      bool IsResizing { get; }

      int MinWidth { get; set; }
      int MaxWidth { get; set; }
      int MinHeight { get; set; }
      int MaxHeight { get; set; }

      void StartResize();
      void Resize(int horizontalDelta, bool isFromLeftSide, int verticalDelta, bool isFromTopSide);
      void CompleteResize();

      ICommand StartResizeCommand { get; }
      Utilities.RelayCommand<ResizeCommandArgs> ResizeCommand { get; }
      ICommand CompleteResizeCommand { get; }
   }

   public struct ResizeCommandArgs
   {
      public int HorizontalDelta;
      public bool IsFromLeftSide;
      public int VerticalDelta;
      public bool IsFromTopSide;
   }

   public enum ResizeType
   {
      None,
      Horizontal,
      Vertical,
      All,
   }
}
