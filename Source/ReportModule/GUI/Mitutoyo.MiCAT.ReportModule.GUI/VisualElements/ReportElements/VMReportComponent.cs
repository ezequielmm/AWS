// <copyright file="VMReportComponent.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Core;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements
{
   public class VMReportComponent : VMVisualElement, IVMReportComponent
   {
      private readonly IDeleteComponentController _deleteComponentController;
      private readonly IAppStateHistory _appStateHistory;

      public VMReportComponent(
         IAppStateHistory appStateHistory,
         IItemId<IReportComponent> reportComponentId,
         IVMReportComponentPlacement vmPlacement,
         IDeleteComponentController deleteComponentController)
         : this(appStateHistory, reportComponentId, vmPlacement, deleteComponentController, null)
      {
      }

      public VMReportComponent(
         IAppStateHistory appStateHistory,
         IItemId<IReportComponent> reportComponentId,
         IVMReportComponentPlacement vmPlacement,
         IDeleteComponentController deleteComponentController,
         TypeFilter[] extraAppStateTypedFilters) : base(vmPlacement)
      {
         _appStateHistory = appStateHistory.AddClient(this);
         _deleteComponentController = deleteComponentController;

         Id = reportComponentId;
         SetDisplayMode(DisplayMode.Normal);

         TypeFilter[] typedFilters = { new TypeFilter(typeof(ReportModeState)), new TypeFilter(typeof(ReportComponentSelection)) };
         if (extraAppStateTypedFilters != null)
            typedFilters = typedFilters.Concat(extraAppStateTypedFilters).ToArray();

         var appStateChangesSubscription = new Subscription(this).With(typedFilters).With(new IdFilter(Id));

         _appStateHistory.Subscribe(appStateChangesSubscription);

         InitializeFromSnapShot(_appStateHistory.CurrentSnapShot);

         DeleteComponentCommand = new RelayCommand<object>(OnDeleteComponent);
      }

      protected virtual void InitializeFromSnapShot(ISnapShot snapShot)
      {
         UpdateFromBusinessEntity(null, snapShot.GetItem(Id));
         UpdateRenderModeFromAppState(snapShot);
         UpdateReportComponentSelection(snapShot);
      }

      public new IVMReportComponentPlacement VMPlacement => (base.VMPlacement as IVMReportComponentPlacement);

      public ICommand DeleteComponentCommand { get; set; }

      Name IHasName.Name => this.GetType().GetTypeName();
      public IItemId<IReportComponent> Id { get; }

      protected virtual void UpdateFromBusinessEntity(IReportComponent reportComponentBefore, IReportComponent reportComponentAfter)
      {
         VMPlacement.UpdateFromPlacementEntity(reportComponentBefore?.Placement, reportComponentAfter.Placement);
      }
      private void UpdateRenderModeFromAppState(ISnapShot snapShot)
      {
         var newRenderMode = snapShot.IsReportInEditMode() ? RenderMode.EditMode : RenderMode.ViewMode;
         SetRenderMode(newRenderMode);
      }
      protected void UpdateReportComponentSelection(ISnapShot snapShot)
      {
         VMPlacement.UpdateSelectionState((RenderMode == RenderMode.EditMode) && snapShot.IsComponentSelected(Id));
      }

      protected virtual void OnUpdate(ISnapShot snapShot)
      {
         UpdateRenderModeFromAppState(snapShot);
         UpdateComponentFromAppState(snapShot);
         UpdateReportComponentSelection(snapShot);
      }

      public Task Update(ISnapShot snapShot)
      {
         OnUpdate(snapShot);
         return Task.CompletedTask;
      }

      public virtual void Dispose()
      {
         _appStateHistory?.RemoveClient(this);
      }

      private void UpdateComponentFromAppState(ISnapShot snapShot)
      {
         var reportComponentChange = GetReportComponentChange(snapShot);

         if (reportComponentChange != null && reportComponentChange is UpdateItemChange)
            UpdateFromBusinessEntity(reportComponentChange.BeforeItem as IReportComponent, reportComponentChange.AfterItem as IReportComponent);
      }

      private IItemChange GetReportComponentChange(ISnapShot snapShot)
      {
         return snapShot.GetChanges().ItemChanges.SingleOrDefault(rc => rc.ItemId.Equals(Id));
      }

      protected virtual void OnDeleteComponent(object arg)
      {
         _deleteComponentController.DeleteComponent(Id);
      }
   }
}
