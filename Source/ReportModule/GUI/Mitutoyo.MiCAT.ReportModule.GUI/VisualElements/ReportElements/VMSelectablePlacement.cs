// <copyright file="VMSelectablePlacement.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements
{
   public abstract class VMSelectablePlacement : VMVisualPlacement, IVMSelectablePlacement
   {
      private bool _isSelectable = true;
      private bool _isSelected;

      public VMSelectablePlacement()
      {
         SelectCommand = new RelayCommand<object>(Select);
      }

      public bool IsSelectable
      {
         get => _isSelectable;
         set
         {
            if (value == _isSelectable) return;

            _isSelectable = value;
            RaisePropertyChanged();
         }
      }

      public bool IsSelected
      {
         get => _isSelected;
         protected set
         {
            if (_isSelected == value) return;
            _isSelected = value;

            RaisePropertyChanged();
         }
      }

      public RelayCommand<object> SelectCommand { get; }

      public void Select(object obj)
      {
         if (IsSelectable && !IsSelected)
            SetSelected();
      }

      protected abstract void SetSelected();
   }
}
