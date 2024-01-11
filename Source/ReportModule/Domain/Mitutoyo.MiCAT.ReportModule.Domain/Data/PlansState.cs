// <copyright file="PlansState.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Immutable;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Common.Contract;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Data
{
   public class PlansState : BaseStateEntity<PlansState>, IUnsaveableEntity
   {
      public PlansState() : this(ImmutableList<PlanDescriptor>.Empty)
      {
      }
      public PlansState(ImmutableList<PlanDescriptor> plans) : this(new UniqueValueFactory(), plans, null, ImmutableList<RunDescriptor>.Empty)
      {
      }

      private PlansState(Id<PlansState> Id, ImmutableList<PlanDescriptor> planList, IDescriptor selectedPlanPart, ImmutableList<RunDescriptor> runList)
         : base(Id)
      {
         PlanList = planList;
         SelectedPlanDataSource = selectedPlanPart;
         RunList = runList;
      }
      public ImmutableList<PlanDescriptor> PlanList { get; }
      public IDescriptor SelectedPlanDataSource { get; }
      public ImmutableList<RunDescriptor> RunList { get; }

      public PlansState WithRunList(ImmutableList<RunDescriptor> newRunList)
      {
         return new PlansState(Id, PlanList, SelectedPlanDataSource, newRunList);
      }

      public PlansState With(IDescriptor selectedPlanPart, ImmutableList<RunDescriptor> newRunList)
      {
         return new PlansState(Id, PlanList, selectedPlanPart, newRunList);
      }

      public PlansState With(ImmutableList<PlanDescriptor> planList)
      {
         return new PlansState(Id, planList, SelectedPlanDataSource, RunList);
      }

      public PlansState WithClearedSelection()
      {
         return new PlansState(Id, PlanList, null, ImmutableList<RunDescriptor>.Empty);
      }
   }
}
