// <copyright file="RunSelectionRequest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Common.Contract;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Data
{
   public class RunSelectionRequest : BaseStateEntity<RunSelectionRequest>, IUnsaveableEntity
   {
      public RunSelectionRequest() :this (Guid.NewGuid(), null, false)
      {
      }
      private RunSelectionRequest (Id<RunSelectionRequest> id, Guid? runRequestedId, bool pending) : base(id)
      {
         RunRequestedId = runRequestedId;
         Pending = pending;
      }

      public Guid? RunRequestedId { get; }
      public bool Pending { get; }

      public RunSelectionRequest WithNewRequest(Guid? runRequestedID)
      {
         return new RunSelectionRequest(Id, runRequestedID, true);
      }
      public RunSelectionRequest WithCompletedRequest()
      {
         return new RunSelectionRequest(Id, RunRequestedId, false);
      }
   }
}
