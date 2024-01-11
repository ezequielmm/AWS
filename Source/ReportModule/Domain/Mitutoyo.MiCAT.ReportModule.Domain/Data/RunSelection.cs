// <copyright file="RunSelection.cs" company="Mitutoyo Europe GmbH">
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
   public class RunSelection : BaseStateEntity<RunSelection>, IUnsaveableEntity
   {
      public static RunSelection PropertiesForAppState(Id<RunSelection> id, Guid? selectedRun) //AppState don't care about RunData
      {
         return new RunSelection(id, selectedRun, null);
      }

      public RunSelection() : this(null, null)
      {
      }
      public RunSelection(Guid? selectedRun, RunData selectedRunData) : this(Guid.NewGuid(), selectedRun, selectedRunData)
      {
      }
      private RunSelection(Id<RunSelection> id, Guid? selectedRun, RunData selectedRunData) : base(id)
      {
         SelectedRun = selectedRun;
         SelectedRunData = selectedRunData;
      }

      public Guid? SelectedRun { get; }
      public RunData SelectedRunData { get; }

      public RunSelection WithNewSelectedRun(Guid newSelectedRun, RunData newRunData)
      {
         return new RunSelection(Id, newSelectedRun, newRunData);
      }
      public RunSelection WithNoSelectedRun()
      {
         return new RunSelection(Id, null, null);
      }
   }
}
