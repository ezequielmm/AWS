// <copyright file="AppStateReportComponentTestHelper.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;

namespace Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests.TestHelper
{
   internal static class AppStateReportComponentTestHelper
   {
      internal static ISnapShot InitializeSnapShot(ISnapShot snapShot)
      {
         snapShot = AddCollections(snapShot);
         snapShot = AddReportSections(snapShot);

         return snapShot;
      }

      internal static IItemId GetReportBodyId(this ISnapShot snapShot)
      {
         return snapShot.GetItems<ReportBody>().Single().Id;
      }

      internal static IItemId GetReportHeaderId(this ISnapShot snapShot)
      {
         return snapShot.GetItems<ReportHeader>().Single().Id;
      }

      internal static IItemId GetReportFooterId(this ISnapShot snapShot)
      {
         return snapShot.GetItems<ReportFooter>().Single().Id;
      }

      private static ISnapShot AddCollections(ISnapShot snapShot)
      {
         snapShot = snapShot.AddCollection<ReportHeader>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportBody>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportFooter>(AppStateKinds.Undoable);

         snapShot = snapShot.AddCollection<ReportImage>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportTableView>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportTextBox>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportTessellationView>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportHeaderForm>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportHeaderFormRow>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportHeaderFormField>(AppStateKinds.Undoable);

         snapShot = snapShot.AddCollection<ReportComponentSelection>(AppStateKinds.Undoable);
         snapShot = snapShot.AddCollection<ReportSectionSelection>(AppStateKinds.Undoable);

         return snapShot;
      }

      private static ISnapShot AddReportSections(ISnapShot snapShot)
      {
         snapShot = snapShot.AddItem(new ReportHeader());
         snapShot = snapShot.AddItem(new ReportBody());
         snapShot = snapShot.AddItem(new ReportFooter());

         snapShot = snapShot.AddItem(new ReportComponentSelection());
         snapShot = snapShot.AddItem(new ReportSectionSelection());

         return snapShot;
      }
   }
}
