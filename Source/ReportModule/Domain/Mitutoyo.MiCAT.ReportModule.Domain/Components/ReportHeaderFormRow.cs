// <copyright file="ReportHeaderFormRow.cs" company="Mitutoyo Europe GmbH">
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
   public class ReportHeaderFormRow : BaseStateEntity<ReportHeaderFormRow>, IReportComponentDataItemsContainer
   {
      public IImmutableList<Id<ReportHeaderFormField>> FieldIds { get; }

      public IEnumerable<IItemId> ReportComponentDataItemIds => FieldIds;

      public ReportHeaderFormRow(IEnumerable<Id<ReportHeaderFormField>> fieldIds)
         : this(Guid.NewGuid(), fieldIds)
      {
      }

      private ReportHeaderFormRow(Id<ReportHeaderFormRow> id, IEnumerable<Id<ReportHeaderFormField>> fieldIds)
         : base(id)
      {
         FieldIds = fieldIds.ToImmutableList();
      }
   }
}
