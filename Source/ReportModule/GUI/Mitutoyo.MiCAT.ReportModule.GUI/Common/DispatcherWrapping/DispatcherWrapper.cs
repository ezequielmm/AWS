// <copyright file="DispatcherWrapper.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Windows;
using System.Windows.Threading;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.DispatcherWrapping
{
   public class DispatcherWrapper : IDispatcherWrapper
   {
      private readonly Dispatcher _dispatcher;

      public DispatcherWrapper()
      {
         _dispatcher = Application.Current.Dispatcher;
      }

      public DispatcherOperation BeginInvokeAfterWPFRenderAsync(Action callback)
      {
         return _dispatcher.BeginInvoke(callback, DispatcherPriority.ContextIdle);
      }

      public void WaitForWPFToRender()
      {
         _dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle);
      }
   }
}
