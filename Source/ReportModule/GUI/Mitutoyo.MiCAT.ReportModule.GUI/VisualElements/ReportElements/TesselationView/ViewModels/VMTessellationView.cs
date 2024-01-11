// <copyright file="VMTessellationView.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.TesselationView.ViewModels
{
   public class VMTessellationView : VMReportComponent
   {
      //private VMCadLayout _vmCadLayout;
      private ITessellationViewController _tessellationController;

      public VMTessellationView(
         IAppStateHistory appStateHistory,
         Id<ReportTessellationView> reportComponentId,
         IVMReportBodyPlacement vmPlacement,
         IDeleteComponentController deleteComponentController,
         ITessellationViewController tessellationController
      ) :
         base(
            appStateHistory,
            reportComponentId,
            vmPlacement,
            deleteComponentController)
      {
         var idCadLayout = appStateHistory.CurrentSnapShot.GetItem<ReportTessellationView>(reportComponentId).CADLayoutId;
         _tessellationController = tessellationController;
         //VmCadLayout = new VMCadLayout(snapShot.GetItem(idCadLayout));
      }

      //public VMCadLayout VmCadLayout
      //{
      //   get => _vmCadLayout;
      //   set { _vmCadLayout = value; RaisePropertyChanged(); }
      //}

      public override void Dispose()
      {
         //VmCadLayout?.Dispose();
         //VmCadLayout = null;

         base.Dispose();
      }
   }
}
