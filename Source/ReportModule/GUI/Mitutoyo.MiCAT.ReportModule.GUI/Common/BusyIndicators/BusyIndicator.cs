// <copyright file="BusyIndicator.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.GUI.Common.DispatcherWrapping;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.BusyIndicators
{
   public class BusyIndicator : VMBase, IBusyIndicator
   {
      private readonly IDispatcherWrapper _dispatcherWrapper;
      private bool _isBusy = false;

      public BusyIndicator(IDispatcherWrapper dispatcherWrapper)
      {
         _dispatcherWrapper = dispatcherWrapper;
      }

      public bool IsBusy
      {
         get => _isBusy;
         private set
         {
            if (_isBusy == value) return;

            _isBusy = value;
            RaisePropertyChanged();
         }
      }

      public void SetIsBusyTrueAndWaitForWPFRender()
      {
         if (!IsBusy)
         {
            IsBusy = true;
            WaitForBusyIndicatorViewRendered();
         }
      }

      public void SetIsBusyFalse()
      {
         IsBusy = false;
      }

      public void SetIsBusyTrueUntilUIIsIdle()
      {
         if (!IsBusy)
         {
            IsBusy = true;
            WaitForBusyIndicatorViewRendered();
            SetIsBusyFalseAfterWPFRender();
         }
      }

      private void WaitForBusyIndicatorViewRendered()
      {
         _dispatcherWrapper.WaitForWPFToRender();
      }

      private void SetIsBusyFalseAfterWPFRender()
      {
         _ = _dispatcherWrapper.BeginInvokeAfterWPFRenderAsync(SetIsBusyFalse);
      }
   }
}
