// <copyright file="UndoRedoController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class UndoRedoController : IUndoRedoController
   {
      private readonly IAppStateHistory _history;

      public bool CanUndo => (_history.CanUndoMany() > 1) && _history.CurrentSnapShot.IsReportInEditMode();
      public bool CanRedo => _history.CanRedo() && _history.CurrentSnapShot.IsReportInEditMode();

      public UndoRedoController(IAppStateHistory history)
      {
         _history = history;
      }

      public Task Undo()
      {
         if (CanUndo)
         {
            return _history.UndoAsync();
         }
         return Task.CompletedTask;
      }

      public Task Redo()
      {
         if (CanRedo)
         {
            return _history.RedoAsync();
         }
         return Task.CompletedTask;
      }
   }
}
