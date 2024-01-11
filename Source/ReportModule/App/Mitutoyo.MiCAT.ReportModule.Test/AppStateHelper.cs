// <copyright file="AppStateHelper.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.ShellModule;

namespace Mitutoyo.MiCAT.ReportModule.Test
{
   [ExcludeFromCodeCoverage]
   public static class AppStateHelper
   {
      public static IAppStateHistory CreateAndInitializeAppStateHistory()
      {
         ISnapShot CollectionAdder(ISnapShot snapShot)
         {
            snapShot = snapShot.AddCollection<NavigationList>(AppStateKinds.Restorable);
            snapShot = snapShot.AddCollection<NavigationModuleChildInfo>(AppStateKinds.Restorable);
            snapShot = snapShot.AddCollection<NavigationModuleInfo>(AppStateKinds.Restorable);
            snapShot = snapShot.AddCollection<NavigationListToModule>(AppStateKinds.Restorable);
            snapShot = snapShot.AddCollection<NavigationModuleToChild>(AppStateKinds.Restorable);
            snapShot = snapShot.AddCollection<SelectedItem>(AppStateKinds.Restorable);
            snapShot = snapShot.AddCollection<Selection>(AppStateKinds.Restorable);
            snapShot = snapShot.AddCollection<NavigationListToSelection>(AppStateKinds.Restorable);
            snapShot = snapShot.AddCollection<PopupWindowEntity>(AppStateKinds.Restorable);
            snapShot = snapShot.AddCollection<PopupSelectionAssociation>(AppStateKinds.Restorable);
            return snapShot;
         }

         var history = new AppStateHistoryFactory().Create(CollectionAdder, "Client", false);
         SetUpAppState(history);
         return history;
      }

      private static void SetUpAppState(IAppStateHistory history)
      {
         var snapShot = history.NextSnapShot(ControllerCall.Empty);
         var nl = new NavigationList(snapShot.UniqueValue);
         snapShot = snapShot.AddItem(nl);
         var selection = new Selection(snapShot.UniqueValue);
         snapShot = snapShot.AddItem(selection);
         var nlToSelection = new NavigationListToSelection(snapShot.UniqueValue, nl.Id, selection.Id);
         snapShot = snapShot.AddItem(nlToSelection);

         var popupWindowSelection = new PopupWindowEntity(snapShot.UniqueValue);
         snapShot = snapShot.AddItem(popupWindowSelection);

         history.AddSnapShot(snapShot);
      }
   }
}
