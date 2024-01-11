// <copyright file="ReportModuleInfoPopulator.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Core.StdLib.Extensions;
using Mitutoyo.MiCAT.ReportModule.Properties;
using Mitutoyo.MiCAT.ShellModule;

namespace Mitutoyo.MiCAT.ReportModule
{
   public class ReportModuleInfoPopulator : IReportModuleInfo
   {
      private readonly IAppStateHistory _history;
      private readonly IReportModuleNavigation _navigation;
      private readonly List<Id> _childItemsId;

      public ReportModuleInfoPopulator(IAppStateHistory history, IReportModuleNavigation navigation)
      {
         _history = history;
         _navigation = navigation;
         _childItemsId = new List<Id>();

         PopulateModuleInformation();
      }

      public Id ModuleId { get; private set; }

      public ImmutableList<Id> ChildIds => _childItemsId.ToImmutableList();

      private void PopulateModuleInformation()
      {
         _history.Run(snapShot => PopulateModuleInformation(snapShot));
      }
      private ISnapShot PopulateModuleInformation(ISnapShot snapShot)
      {
         var assemblyName = typeof(ReportModuleInfoPopulator).Assembly.FullName;

         var module = new NavigationModuleInfo(snapShot.NewUniqueValue(), Resources.Report, "Navigation/Images/ReportWorkspace_icon.xaml", "ReportWorkspace_iconDrawingImage", assemblyName, "R");
         ModuleId = module.Id;

         var nl = snapShot.GetItems<NavigationList>().Single();

         var moduleOrdinal = GetNextModuleOrdinal(snapShot, nl.Id);
         var listtoModule = new NavigationListToModule(snapShot.NewUniqueValue(), nl.Id, module.Id, moduleOrdinal);
         return snapShot.AddItem(module).AddItem(listtoModule);
      }

      private static Ordinal GetNextModuleOrdinal(ISnapShot snapShot, Id<NavigationList> navigationList)
      {
         var moduleOrdinals = snapShot.GetBackReferences<NavigationListToModule>(navigationList).Select(i => i.Order);
         return moduleOrdinals.IsEmpty() ? 0 : moduleOrdinals.Max() + 1;
      }

      public ISnapShot AddChildItems(NavigationModuleInfo moduleInfo, ISnapShot snapShot, NavigationModuleChildInfo childInfo)
      {
         snapShot = snapShot.AddItem(childInfo);
         _navigation.AddNewInstanceToRegion(childInfo.Id);
         return UpdateSnapshot(snapShot, moduleInfo, childInfo);
      }

      private ISnapShot UpdateSnapshot(ISnapShot snapShot, NavigationModuleInfo moduleInfo,
         NavigationModuleChildInfo childInfo)
      {
         _childItemsId.Add(childInfo.Id);
         var order = GetNextOrdinal(snapShot, moduleInfo.Id);

         var moduleToChild = new NavigationModuleToChild(snapShot.NewUniqueValue(), moduleInfo.Id, childInfo.Id, order);
         snapShot = snapShot.AddItem(moduleToChild);
         snapShot = ClearAndAddSelection(snapShot, childInfo.Id);
         return snapShot;
      }

      public NavigationModuleChildInfo CreateChildItem()
      {
         var snapShot = _history.CurrentSnapShot;
         var childNamesList = snapShot.GetItems<NavigationModuleChildInfo>().Select(x => x.Name).ToList();
         var existingAndNewReport = childNamesList;
         existingAndNewReport.Add(Resources.ReportNavigation);
         return new NavigationModuleChildInfo(snapShot.NewUniqueValue(), UniqueNameFactory.CreateUniqueName(Resources.ReportNavigation, existingAndNewReport));
      }

      private string GetReportName(string name)
      {
         return string.IsNullOrEmpty(name) ? $"{Resources.ReportNavigation} {_childItemsId.Count + 1}" : name;
      }

      public void RemoveChildItem(Id id)
      {
         _childItemsId.Remove(id);
      }

      private ISnapShot ClearAndAddSelection(ISnapShot snapShot, Id selectedItemId)
      {
         foreach (var entity in snapShot.GetItems<SelectedItem>())
         {
            snapShot = snapShot.DeleteItem(entity);
         }

         var navigationList = snapShot.GetItems<NavigationList>().Single();
         var nts = snapShot.GetItems<NavigationListToSelection>().Single(i => i.SourceId.Equals(navigationList.Id));
         var selection = snapShot.GetItem(nts.TargetId);

         var selectedItem = new SelectedItem(snapShot.UniqueValue, selection.Id, selectedItemId, 0);

         return snapShot.AddItem(selectedItem);
      }

      private static Ordinal GetNextOrdinal(ISnapShot snapShot, Id<NavigationModuleInfo> parentId)
      {
         var lastItem = snapShot.GetItems<NavigationModuleToChild>()
            .Where(m => m.SourceId == parentId)
            .OrderBy(i => i.Order).LastOrDefault();

         return lastItem != null ? lastItem.Order.Value + 1 : 0;
      }

      public bool ChildIdExists(Id reportId)
      {
         return ChildIds.Contains(reportId);
      }
   }
}