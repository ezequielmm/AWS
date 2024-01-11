// <copyright file="ReportModeState.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Common.Contract;

namespace Mitutoyo.MiCAT.ReportModule.App.AppState
{
   public class ReportModeState : BaseStateEntity<ReportModeState>, IUnsaveableEntity
   {
      public ReportModeState() : this(false)
      {
      }
      public ReportModeState(bool editMode) : this(new Id<ReportModeState>(Guid.NewGuid()), editMode)
      {
      }

      private ReportModeState(Id<ReportModeState> id, bool editMode) : base(id)
      {
         EditMode = editMode;
      }

      public bool EditMode { get; }

      public ReportModeState With(bool editMode)
      {
         return new ReportModeState(Id, editMode);
      }
   }
}
