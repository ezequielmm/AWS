// <copyright file="HeaderFormFieldController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public class HeaderFormFieldController : IHeaderFormFieldController
   {
      public HeaderFormFieldController(IAppStateHistory appStateHistory)
      {
         AppStateHistory = appStateHistory;
      }

      private IAppStateHistory AppStateHistory { get; }

      public void UpdateSelectedHeaderFormField(IItemId<ReportHeaderFormField> reportHeaderFormFieldId, Guid selectedFieldId)
      {
         AppStateHistory.RunUndoable(snapShot => UpdateSelectedHeaderFormField(snapShot, reportHeaderFormFieldId, selectedFieldId));
      }

      private ISnapShot UpdateSelectedHeaderFormField(ISnapShot snapShot, IItemId<ReportHeaderFormField> reportHeaderFormFieldId, Guid selectedFieldId)
      {
         var oldItem = snapShot.GetItem(reportHeaderFormFieldId);

         if ((oldItem != null) && oldItem.SelectedFieldId != selectedFieldId)
         {
            return snapShot.UpdateItem(oldItem, oldItem.WithSelectedFieldId(selectedFieldId));
         }
         return snapShot;
      }

      public void UpdateSelectedHeaderFormFieldLabel(IItemId<ReportHeaderFormField> reportHeaderFormFieldId, string label)
      {
         AppStateHistory.RunUndoable(snapShot => UpdateSelectedHeaderFormFieldLabel(snapShot, reportHeaderFormFieldId, label));
      }
      private ISnapShot UpdateSelectedHeaderFormFieldLabel(ISnapShot snapShot, IItemId<ReportHeaderFormField> reportHeaderFormFieldId, string label)
      {
         var oldItem = snapShot.GetItem(reportHeaderFormFieldId);

         if (oldItem.CustomLabel != label)
         {
            return snapShot.UpdateItem(oldItem, oldItem.WithCustomLabel(label));
         }
         return snapShot;
      }

      public void UpdateSelectedHeaderFormFieldLabelWidthPercentage(IItemId<ReportHeaderFormField> reportHeaderFormFieldId, double labelWidthPercentage)
      {
         AppStateHistory.RunUndoable(snapShot => UpdateSelectedHeaderFormFieldLabelWidthPercentage(snapShot, reportHeaderFormFieldId, labelWidthPercentage));
      }
      private ISnapShot UpdateSelectedHeaderFormFieldLabelWidthPercentage(ISnapShot snapShot, IItemId<ReportHeaderFormField> reportHeaderFormFieldId, double labelWidthPercentage)
      {
         var oldItem = AppStateHistory.CurrentSnapShot.GetItem(reportHeaderFormFieldId);

         if (oldItem.LabelWidthPercentage != labelWidthPercentage)
         {
            return snapShot.UpdateItem(oldItem, oldItem.WithLabelWidthPercentage(labelWidthPercentage));
         }
         return snapShot;
      }
   }
}
