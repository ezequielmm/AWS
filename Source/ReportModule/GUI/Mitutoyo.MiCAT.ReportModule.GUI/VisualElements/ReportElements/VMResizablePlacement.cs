// <copyright file="VMResizablePlacement.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows.Input;
using Mitutoyo.MiCAT.Common.GUI.Commands;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements
{
   public abstract class VMResizablePlacement : VMDraggablePlacement, IVMResizablePlacement
   {
      private int _minWidth = 0;
      private int _minHeight = 0;
      private int _maxWidth = int.MaxValue;
      private int _maxHeight = int.MaxValue;
      private ResizeType _resizeType = ResizeType.All;
      private bool _isResizing;

      public VMResizablePlacement()
      {
         StartResizeCommand = new RelayCommand(x => StartResize());
         ResizeCommand = new RelayCommand<ResizeCommandArgs>(x =>
            Resize(x.HorizontalDelta, x.IsFromLeftSide, x.VerticalDelta, x.IsFromTopSide));
         CompleteResizeCommand = new RelayCommand(x => CompleteResize());
      }

      public int MinWidth
      {
         get => _minWidth;
         set
         {
            if (value == _minWidth) return;

            _minWidth = value;
            RaisePropertyChanged();
         }
      }

      public int MaxWidth
      {
         get => _maxWidth;
         set
         {
            if (value == _maxWidth) return;

            _maxWidth = value;
            RaisePropertyChanged();
         }
      }

      public int MinHeight
      {
         get => _minHeight;
         set
         {
            if (value == _minHeight) return;

            _minHeight = value;
            RaisePropertyChanged();
         }
      }

      public int MaxHeight
      {
         get => _maxHeight;
         set
         {
            if (value == _maxHeight) return;

            _maxHeight = value;
            RaisePropertyChanged();
         }
      }

      public bool IsResizable => ResizeType != ResizeType.None;

      public ResizeType ResizeType
      {
         get => _resizeType;
         set
         {
            if (value == _resizeType) return;

            _resizeType = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(IsResizable));
         }
      }

      public bool IsResizing
      {
         get => _isResizing;
         private set
         {
            _isResizing = value;
            RaisePropertyChanged();
         }
      }

      public bool IsHorizontalResizable =>
         ResizeType == ResizeType.Horizontal || ResizeType == ResizeType.All;

      public bool IsVerticalResizable =>
         ResizeType == ResizeType.Vertical || ResizeType == ResizeType.All;

      public ICommand StartResizeCommand { get; }

      public RelayCommand<ResizeCommandArgs> ResizeCommand { get; }

      public ICommand CompleteResizeCommand { get; }

      public virtual void StartResize()
      {
         if (!IsResizable || IsResizing) return;

         IsResizing = true;
      }

      public void Resize(int horizontalDelta, bool isFromLeftSide, int verticalDelta, bool isFromTopSide)
      {
         if (!IsResizable || !IsResizing) return;

         if (IsHorizontalResizable)
         {
            if (isFromLeftSide)
               ResizeHorizontallyFromLeftSide(horizontalDelta);
            else
               ResizeHorizontallyFromRightSide(horizontalDelta);
         }

         if (IsVerticalResizable)
         {
            if (isFromTopSide)
               ResizeVerticallyFromTopSide(verticalDelta);
            else
               ResizeVerticallyFromBottomSide(verticalDelta);
         }
      }

      public virtual void CompleteResize()
      {
         if (!IsResizable || !IsResizing) return;

         IsResizing = false;
      }

      private void ResizeHorizontallyFromRightSide(int delta)
      {
         var newWidth = VisualWidth + delta;

         if (newWidth < MinWidth)
            newWidth = MinWidth;

         if (newWidth > MaxWidth)
            newWidth = MaxWidth;

         VisualWidth = newWidth;
      }

      private void ResizeHorizontallyFromLeftSide(int delta)
      {
         var newWidth = VisualWidth + delta;

         if (newWidth < MinWidth)
            newWidth = MinWidth;

         if (newWidth > MaxWidth)
            newWidth = MaxWidth;

         var newVisualX = VisualX - (newWidth - VisualWidth);
         VisualX = newVisualX;
         VisualWidth = newWidth;
      }

      private void ResizeVerticallyFromBottomSide(int delta)
      {
         var newHegiht = VisualHeight + delta;

         if (newHegiht < MinHeight)
            newHegiht = MinHeight;

         if (newHegiht > MaxHeight)
            newHegiht = MaxHeight;

         VisualHeight = newHegiht;
      }

      private void ResizeVerticallyFromTopSide(int delta)
      {
         var newHeight = VisualHeight + delta;

         if (newHeight < MinHeight)
            newHeight = MinHeight;

         if (newHeight > MaxHeight)
            newHeight = MaxHeight;

         var newVisualY = VisualY - (newHeight - VisualHeight);
         VisualY = newVisualY;
         VisualHeight = newHeight;
      }
   }
}
