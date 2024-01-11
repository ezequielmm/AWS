// <copyright file="ViewVisibilityController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Visibility;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class ViewVisibilityController : IViewVisibilityController
   {
      private readonly IAppStateHistory _appStateHistory;

      public ViewVisibilityController(IAppStateHistory appStateHistory)
      {
         _appStateHistory = appStateHistory;
      }

      public void ToggleVisibility(ViewElement viewElement)
      {
         ChangeVisibility(viewElement, (vv) => vv.WithToggle());
      }

      public void UpdateVisibility(ViewElement viewElement, bool isVisible)
      {
         ChangeVisibility(viewElement, (vv) => vv.WithVisible(isVisible));
      }

      private void ChangeVisibility(ViewElement viewElement, Func<ViewVisibility, ViewVisibility> newViewVisibilityFunc)
      {
         var oldViewVisibility = GetCurrentViewVisibility(viewElement);
         var newViewVisibility = newViewVisibilityFunc(oldViewVisibility);

         _appStateHistory.Run(snapShot => snapShot.UpdateItem(oldViewVisibility, newViewVisibility));
      }

      private ViewVisibility GetCurrentViewVisibility(ViewElement viewElement)
      {
         return _appStateHistory.CurrentSnapShot.GetViewVisibility(viewElement);
      }
   }
}
