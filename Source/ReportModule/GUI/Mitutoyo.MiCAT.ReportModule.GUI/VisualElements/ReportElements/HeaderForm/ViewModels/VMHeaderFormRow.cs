// <copyright file="VMHeaderFormRow.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Clients;
using Mitutoyo.MiCAT.Core;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels
{
   public class VMHeaderFormRow : VMBase, IUpdateClient, IDisposable
   {
      private readonly IAppStateHistory _appStateHistory;

      public Name Name => this.GetType().GetTypeName();

      public IItemId<ReportHeaderFormRow> RowId { get; private set; }
      public int RowIndex { get; private set; }
      public IVMHeaderForm HeaderForm { get; private set; }
      public List<VMHeaderFormField> Fields { get; }

      public VMHeaderFormRow(
         IAppStateHistory appStateHistory,
         IItemId<ReportHeaderFormRow> rowId,
         int rowIndex,
         IVMHeaderForm headerForm)
      {
         RowId = rowId;
         RowIndex = rowIndex;
         HeaderForm = headerForm;
         _appStateHistory = appStateHistory.AddClient(this);
         Fields = new List<VMHeaderFormField>();
         var appStateSubscription = new Subscription(this).With(new IdFilter(RowId));
         _appStateHistory.Subscribe(appStateSubscription);

         Update(_appStateHistory.CurrentSnapShot);
      }

      public Task Update(ISnapShot snapShot)
      {
         OnUpdate(snapShot);
         return Task.CompletedTask;
      }

      public virtual void Dispose()
      {
         _appStateHistory?.RemoveClient(this);
         Fields.ForEach(x => x.Dispose());
      }

      private void OnUpdate(ISnapShot snapShot)
      {
         if (!snapShot.HasBeenDeleted(RowId))
            UpdateFromSnapShot(snapShot);
      }

      private void UpdateFromSnapShot(ISnapShot snapShot)
      {
         var row = snapShot.GetItem<ReportHeaderFormRow>(RowId);

         var fieldIdsToAdd = row.FieldIds.Except(Fields.Select(x => x.FieldId));
         var fieldIdsToDelete = Fields.Select(x => x.FieldId).Except(row.FieldIds);

         fieldIdsToAdd.ToList().ForEach(x => AddField(x));
         fieldIdsToDelete.ToList().ForEach(x => RemoveField(x));
      }

      private void AddField(IItemId<ReportHeaderFormField> fieldId)
      {
         Fields.Add(new VMHeaderFormField(_appStateHistory, HeaderForm.HeaderFormFieldController, fieldId, Fields.Count(), this));
      }

      private void RemoveField(IItemId<ReportHeaderFormField> fieldId)
      {
         var fieldIndex = Fields.FindIndex(x => x.FieldId == fieldId);
         var field = Fields.ElementAt(fieldIndex);
         Fields.RemoveAt(fieldIndex);
         field.Dispose();

         // TODO: update index of fields after fieldIndex
      }
   }
}
