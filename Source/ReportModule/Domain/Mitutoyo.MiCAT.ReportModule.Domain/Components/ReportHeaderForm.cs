// <copyright file="ReportHeaderForm.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Mitutoyo.MiCAT.ApplicationState;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Components
{
   public class ReportHeaderForm : ReportComponentBase<ReportHeaderForm>, IReportComponent, IReportComponentDataItemsContainer
   {
      private const int INITIAL_HEADER_FORM_WIDTH = 340;
      private const int INITIAL_HEADER_FORM_HEIGHT = 32;

      public ReportHeaderForm(IItemId reportSectionId, int x, int y, IEnumerable<Id<ReportHeaderFormRow>> rowIds)
         : this (new ReportComponentPlacement(reportSectionId, x, y, INITIAL_HEADER_FORM_WIDTH, INITIAL_HEADER_FORM_HEIGHT), rowIds)
      { }
      public ReportHeaderForm(ReportComponentPlacement placement, IEnumerable<Id<ReportHeaderFormRow>> rowIds)
         : this(Guid.NewGuid(), placement, rowIds)
      { }

      public ReportHeaderForm(Id<ReportHeaderForm> id, ReportComponentPlacement placement, IEnumerable<Id<ReportHeaderFormRow>> rowIds)
         : base(id, placement)
      {
         RowIds = rowIds.ToImmutableList();
      }

      public IImmutableList<Id<ReportHeaderFormRow>> RowIds { get; }

      public IEnumerable<IItemId> ReportComponentDataItemIds => RowIds;

      public override ReportHeaderForm WithPosition(int x, int y)
      {
         return new ReportHeaderForm(Id, Placement.WithPosition(x, y), RowIds) ;
      }

      public override ReportHeaderForm WithSize(int widht, int height)
      {
         return new ReportHeaderForm(Id, Placement.WithSize(widht, height), RowIds);
      }
   }
}
