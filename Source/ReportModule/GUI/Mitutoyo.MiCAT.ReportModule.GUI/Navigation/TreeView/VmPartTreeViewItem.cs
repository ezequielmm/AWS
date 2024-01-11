// <copyright file="VmPartTreeViewItem.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Navigation.TreeView
{
   public class VmPartTreeViewItem : VmBaseTreeViewItem
   {
      private readonly IPlanController _planController;
      private readonly IActionCaller _actionCaller;

      public VmPartTreeViewItem(
         IPlanController planController,
         IActionCaller actionCaller)
      {
         _planController = planController;
         _actionCaller = actionCaller;

         CanDeleteItem = false;
      }

      protected override void OnSelectItem()
      {
         _actionCaller.RunUserActionAsync(() => _planController.SelectPart(Id));
      }
   }
}
