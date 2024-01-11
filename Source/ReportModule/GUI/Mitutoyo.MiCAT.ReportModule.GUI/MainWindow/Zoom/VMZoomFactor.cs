// <copyright file="VMZoomFactor.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.Zoom
{
   public class VMZoomFactor : VMBase, IVMZoomFactor
   {
      private const double MIN_SCALE = 0.1;
      private const double MAX_SCALE = 4.0;
      private const double SCALE_STEP = 0.1;

      private double _scale;

      public ICommand ZoomCommand { get; private set; }
      public ICommand SelectionChangedCommand { get; private set; }

      public IList<VMZoomLevel> ZoomOptions => GetZoomOptions();

      private IList<VMZoomLevel> GetZoomOptions()
      {
         return new List<VMZoomLevel>()
         {
            new VMZoomLevel(2),
            new VMZoomLevel(1.5),
            new VMZoomLevel(1.25),
            new VMZoomLevel(1),
            new VMZoomLevel(0.75),
            new VMZoomLevel(0.5),
            new VMZoomLevel(0.25)
         };
      }

      public VMZoomFactor()
      {
         ZoomCommand = new RelayCommand<ZoomInOutCommandArgs>(OnZoom);
         SelectionChangedCommand = new RelayCommand<VMZoomLevel>(OnSelectionChanged);

         Scale = 1.0;
      }

      private void OnSelectionChanged(VMZoomLevel option)
      {
         if (option != null)
            Scale = option.Value;

         Keyboard.ClearFocus();
      }

      public double Scale
      {
         get => _scale;
         set
         {
            if (_scale != value && IsScaleValueInsideValidRange(value))
            {
               _scale = value;
               RaisePropertyChanged();
            }
         }
      }

      private bool IsScaleValueInsideValidRange(double scaleValue)
      {
         return scaleValue >= MIN_SCALE && scaleValue <= MAX_SCALE;
      }

      private void OnZoom(ZoomInOutCommandArgs args)
      {
         double newScale = ZommInOutScale(args);
         if ((MIN_SCALE <= newScale) && (newScale <= MAX_SCALE))
         {
            Scale = newScale;
         }
      }

      private double ZommInOutScale(ZoomInOutCommandArgs args)
      {
         int sign = args.ZoomCommand == Operation.ZoomIn ? 1 : -1;
         return Math.Round(_scale + sign * SCALE_STEP, 1);
      }
   }
}
