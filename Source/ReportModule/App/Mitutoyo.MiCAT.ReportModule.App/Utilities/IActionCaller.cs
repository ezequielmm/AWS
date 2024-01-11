// <copyright file="IActionCaller.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Threading.Tasks;

namespace Mitutoyo.MiCAT.ReportModuleApp.Utilities
{
   public interface IActionCaller
   {
      /// <summary>
      /// Starts showing busy indicator (wait until WPF render it) and remove it after the async Task has finished
      /// </summary>
      /// <param name="func">Async operation to run with the busy indicator On</param>
      /// <remarks>
      /// Intended to be user for User operations which won't allow a second user operation until the first operation ends. Not for internal calculations.
      /// Calling this method again while the func for the first call is still running, can cause unexpected behavior.
      /// </remarks>
      /// <returns>Returns the same task that the func parameter returns</returns>
      Task RunUserActionAsync(Func<Task> func);

      /// <summary>
      /// Starts showing busy indicator (wait until WPF render it) and remove it after the action has finished
      /// </summary>
      /// <remarks>
      /// Intended to be user for User operations which won't allow a second user operation until the first operation ends. Not for internal calculations.
      /// Is the same as RunUserActionAsync but for operation that releases the UI thread and don't have a Task to await for. (Eg: Opening a FileDialog)
      /// Calling this method again while the func for the first call is still running, can cause unexpected behavior.
      /// </remarks>
      /// <param name="func">Operation to run with the busy indicator On</param>
      void RunUserAction(Action func);

      /// <summary>
      /// Starts showing busy indicator (wait until WPF render it) and queue the switch off for it by when WPF Render works has ended.
      /// </summary>
      /// <remarks>
      /// Intended to be used for long operations on the UI thread. Once the UI thread is idle, the busy indicator will be switched off.
      /// Can be used for user actions that not release the UI thread until the action is finished.
      /// Can be called multiple times and will not wait for render each time if BusyIndicator is currently on. (RunUserAction will wait for the render everytime.)
      /// </remarks>
      /// <param name="func">Long render UI operation</param>
      void RunUIThreadAction(Action func);
   }
}
