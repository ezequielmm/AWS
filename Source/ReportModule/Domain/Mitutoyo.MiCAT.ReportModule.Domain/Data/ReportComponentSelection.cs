// <copyright file="ReportComponentSelection.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Common.Contract;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Data
{
   public class ReportComponentSelection : BaseStateEntity<ReportComponentSelection>, IUnsaveableEntity
   {
      public ReportComponentSelection() :
         this(new List<IItemId<IReportComponent>>())
      {
      }

      public ReportComponentSelection(IEnumerable<IItemId<IReportComponent>> selectedReportComponentIds) :
         this (
            new Id<ReportComponentSelection>(Guid.NewGuid()),
            selectedReportComponentIds
         )
      {
      }

      private ReportComponentSelection(Id<ReportComponentSelection> id, IEnumerable<IItemId<IReportComponent>> selectedReportComponentIds) : base(id)
      {
         SelectedReportComponentIds = selectedReportComponentIds;
      }

      public IEnumerable<IItemId<IReportComponent>> SelectedReportComponentIds { get; }

      public ReportComponentSelection WithJustThisSelectedComponent(IItemId<IReportComponent> reportComponentSelectedIds)
      {
         return new ReportComponentSelection(Id, new List<IItemId<IReportComponent>>() { reportComponentSelectedIds });
      }

      public ReportComponentSelection WithNonSelectedComponent()
      {
         return new ReportComponentSelection(Id, new List<IItemId<IReportComponent>>());
      }

      public ReportComponentSelection WithThisIdComponentUnselected(IItemId<IReportComponent> reportComponentToUnselectId)
      {
         var newSelectedComponentsId = (new List<IItemId<IReportComponent>>(SelectedReportComponentIds));
         newSelectedComponentsId.Remove(reportComponentToUnselectId);

         return new ReportComponentSelection(Id, newSelectedComponentsId);
      }
   }
}
