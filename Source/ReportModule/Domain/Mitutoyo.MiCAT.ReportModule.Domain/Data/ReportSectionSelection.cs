// <copyright file="ReportSectionSelection.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Common.Contract;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Data
{
   public class ReportSectionSelection : BaseStateEntity<ReportSectionSelection>, IUnsaveableEntity
   {
      public ReportSectionSelection() : this(new List<IItemId>())
      {
      }

      public ReportSectionSelection(List<IItemId> selectedSectionIds) :
         this(new Id<ReportSectionSelection>(Guid.NewGuid()), selectedSectionIds)
      {
      }

      private ReportSectionSelection(Id<ReportSectionSelection> id, List<IItemId> selectedSectionIds) : base(id)
      {
         SelectedSectionIds = selectedSectionIds;
      }

      public List<IItemId> SelectedSectionIds { get; internal set; }

      public ReportSectionSelection WithNewSelectedSectionsList(List<IItemId> newList)
      {
         return new ReportSectionSelection(Id, newList);
      }
   }
}
