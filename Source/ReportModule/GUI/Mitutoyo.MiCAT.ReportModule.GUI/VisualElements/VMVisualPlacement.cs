// <copyright file="VMVisualPlacement.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.GUI.Common;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements
{
   public class VMVisualPlacement : VMBase, IVMVisualPlacement
   {
      private int _visualX;
      private int _visualY;
      private int _visualWidth;
      private int _visualHeight;
      private bool _autoAdjustVisualHeight;
      private bool _autoAdjustVisualWidth;

      public int VisualX
      {
         get => _visualX;
         protected set
         {
            if (value == _visualX) return;

            _visualX = value;
            RaisePropertyChanged();
         }
      }

      public int VisualY
      {
         get => _visualY;
         protected set
         {
            if (value == _visualY)
               return;

            _visualY = value;
            RaisePropertyChanged();
         }
      }

      public int VisualWidth
      {
         get => _visualWidth;
         protected set
         {
            if (value == _visualWidth)
               return;

            _visualWidth = value;
            RaisePropertyChanged();
         }
      }

      public int VisualHeight
      {
         get => _visualHeight;
         protected set
         {
            if (value == _visualHeight)
               return;

            _visualHeight = value;
            RaisePropertyChanged();
         }
      }

      public bool AutoAdjustVisualWidth
      {
         get => _autoAdjustVisualWidth;
         protected set
         {
            if (value == _autoAdjustVisualWidth)
               return;

            _autoAdjustVisualWidth = value;
            RaisePropertyChanged();
         }
      }

      public bool AutoAdjustVisualHeight
      {
         get => _autoAdjustVisualHeight;
         protected set
         {
            if (value == _autoAdjustVisualHeight)
               return;

            _autoAdjustVisualHeight = value;
            RaisePropertyChanged();
         }
      }

      public virtual void SetVisualPosition(int visualX, int visualY)
      {
         VisualX = visualX;
         VisualY = visualY;
      }

      public virtual void SetVisualSize(int visualWidth, int visualHeight)
      {
         VisualWidth = visualWidth;
         VisualHeight = visualHeight;
      }
   }
}
