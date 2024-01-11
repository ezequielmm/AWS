// <copyright file="VMHeaderForm.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModuleApp.AppState;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels
{
   public class VMHeaderForm : VMReportComponent, IVMHeaderForm
   {
      private readonly IAppStateHistory _appStateHistory;
      private IEnumerable<VMDynamicPropertyItem> _dynamicPropertyItems;

      public List<VMHeaderFormRow> Rows { get; }

      public IEnumerable<VMDynamicPropertyItem> DynamicPropertyItems
      {
         get => _dynamicPropertyItems;
         set
         {
            _dynamicPropertyItems = value;
            RaisePropertyChanged();
         }
      }

      public IHeaderFormFieldController HeaderFormFieldController { get; private set; }
      public IHeaderFormController HeaderFormController { get; private set; }

      public VMHeaderForm(
         IAppStateHistory appStateHistory,
         Id<ReportHeaderForm> reportComponentId,
         IVMReportComponentPlacement vmPlacement,
         IDeleteComponentController deleteComponentController,
         IHeaderFormController headerFormController,
         IHeaderFormFieldController headerFormFieldController) :
            base(
               appStateHistory,
               reportComponentId,
               vmPlacement,
               deleteComponentController)
      {
         _appStateHistory = appStateHistory;
         HeaderFormController = headerFormController;
         HeaderFormFieldController = headerFormFieldController;

         VMPlacement.MinWidth = 340;
         VMPlacement.MinHeight = 32;

         PopulateDynamicPropertyItems(_appStateHistory.CurrentSnapShot);

         Rows = new List<VMHeaderFormRow>();
         InitializeRows(_appStateHistory.CurrentSnapShot);
      }
      protected override void UpdateFromBusinessEntity(IReportComponent reportComponentBefore, IReportComponent reportComponentAfter)
      {
         base.UpdateFromBusinessEntity(reportComponentBefore, reportComponentAfter);
         if (Rows != null)
            UpdateRows(reportComponentAfter as ReportHeaderForm);
      }
      private void InitializeRows(ISnapShot snapShot)
      {
         UpdateRows(snapShot.GetItem<ReportHeaderForm>(Id));
      }

      private void UpdateRows(ReportHeaderForm headerForm)
      {
         var rowIdsToAdd = headerForm.RowIds.Except(Rows.Select(x => x.RowId));
         var rowIdsToDelete = this.Rows.Select(x => x.RowId).Except(headerForm.RowIds);

         rowIdsToAdd.ToList().ForEach(x => AddRow(x));
         rowIdsToDelete.ToList().ForEach(x => RemoveRow(x));
      }

      private void AddRow(IItemId<ReportHeaderFormRow> rowId)
      {
         Rows.Add(new VMHeaderFormRow(_appStateHistory, rowId, Rows.Count(), this));
      }

      private void RemoveRow(IItemId<ReportHeaderFormRow> rowId)
      {
         var rowIndex = Rows.FindIndex(x => x.RowId == rowId);
         var row = Rows.ElementAt(rowIndex);
         Rows.RemoveAt(rowIndex);
         row.Dispose();

         // TODO: update index of rows after rowIndex
      }

      private void PopulateDynamicPropertyItems(ISnapShot snapShot)
      {
         var dynamicPropertiesState = snapShot.GetItems<DynamicPropertiesState>().First();
         DynamicPropertyItems = dynamicPropertiesState
            .DynamicProperties
            .Select(x => new VMDynamicPropertyItem
            {
               Id = x.Id,
               EntityType = x.EntityType,
               Name = x.Name,
               DisplayName = LocalizeDynamicPropertyItemName(x.Name),
            }).OrderBy(x => x.DisplayName)
            .Prepend(VMDynamicPropertyItem.EmptyDynamicPropertyItem);
      }

      private string LocalizeDynamicPropertyItemName(string value) =>
         Resources.ResourceManager.GetString($"FieldItem_{value}") ?? value;

      public override void Dispose()
      {
         base.Dispose();
         Rows.ForEach(x => x.Dispose());
      }
   }
}
