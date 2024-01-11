// <copyright file="VMTableFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Components.TableView
{
   [ExcludeFromCodeCoverage]
   public class VMTableFake : VMTable
   {
      public VMTableFake(
         IAppStateHistory history,
         Id<ReportTableView> tableId,
         IVMDisableSpacePlacement vmPlacement,
         IDeleteComponentController deleteComponentController,
         ITableViewController tableViewController)
         : base (
            history,
            tableId,
            vmPlacement,
            deleteComponentController,
            tableViewController)
      {
      }

      public void ExecuteSetNewDataSource(IEnumerable<VMEvaluatedCharacteristic> newDataSource)
      {
         SetNewDataSource(newDataSource);
      }

      public void ExecuteSetNewColumnInfoProperty(ColumnInfoList newColumnInfo)
      {
         SetNewColumnInfoProperty(newColumnInfo);
      }
   }
}
