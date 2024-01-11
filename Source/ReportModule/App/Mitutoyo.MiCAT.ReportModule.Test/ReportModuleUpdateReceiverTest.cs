// <copyright file="ReportModuleUpdateReceiverTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ShellModule;
using Mitutoyo.MiCAT.ShellModule.Regions;
using Moq;
using NUnit.Framework;
using Prism.Regions;

namespace Mitutoyo.MiCAT.ReportModule.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportModuleUpdateReceiverTest
   {
      private IRegionManager _regionManager;
      private IReportModule _reportModule;
      private IAppStateHistory _history;
      private IReportModuleInfo _reportModuleInfo;
      private Selection _selection;
      private IReportModuleNavigation _reportModuleNavigation;

      [SetUp]
      public void Init()
      {
         _history = AppStateHelper.CreateAndInitializeAppStateHistory();
         _regionManager = Mock.Of<IRegionManager>();
         _reportModule = Mock.Of<IReportModule>();
         _reportModuleInfo = new ReportModuleInfoPopulator(_history, new ReportModuleNavigation(_regionManager, _reportModule));
         _selection = new Selection(_history.UniqueValueFactory.UniqueValue());
         _history.Run(snapshot => AddSelection(snapshot, _selection));
         _reportModuleNavigation = new ReportModuleNavigation(_regionManager, _reportModule);
      }
      private ISnapShot AddSelection(ISnapShot snapShot, Selection selection)
      {
         return snapShot.AddItem(selection);
      }

      [Test]
      public void ReportModuleUpdateCallTest()
      {
         new ReportModuleUpdateReceiver(_history, _reportModuleInfo, _reportModuleNavigation);
         SetUpSelectedItem(_history.UniqueValueFactory, _selection, _reportModuleInfo.ModuleId);

         Mock.Get(_regionManager).Setup(r => r.Regions[RegionNames.ToolBarRegion].ActiveViews)
            .Returns(new ViewsCollection(new ObservableCollection<ItemMetadata>(), metadata => false));

         Assert.IsNotEmpty(_history.CurrentSnapShot.GetItems<NavigationListToSelection>());
         Assert.IsNotEmpty(_history.CurrentSnapShot.GetItems<SelectedItem>());
      }

      [Ignore("Issue introduce on build: http://euclid.microen.local:8080/tfs/MiCAT/MiCAT/_build/index?buildId=11743&_a=summary ")]
      [Test]
      [Apartment(ApartmentState.STA)]
      public void ReportModuleChildUpdateCall_Test()
      {
         var selectedItem = SetUpSelectedItem(_history.UniqueValueFactory, _selection, _reportModuleInfo.ChildIds.First());

         new ReportModuleUpdateReceiver(_history, _reportModuleInfo, _reportModuleNavigation);

         Mock.Get(_regionManager)
            .Setup(r => r.Regions[It.IsAny<string>()].GetView(selectedItem.TargetId.ToString())).Returns(null);
         Mock.Get(_regionManager)
            .Setup(r => r.Regions[It.IsAny<string>()].Add(It.IsAny<object>(), It.IsAny<string>())).Callback(() => { });

         Mock.Get(_regionManager)
            .Verify(r => r.Regions[It.IsAny<string>()].GetView(selectedItem.TargetId.ToString()), Times.Exactly(4));
         Mock.Get(_regionManager)
            .Verify(r => r.Regions[It.IsAny<string>()].Activate(It.IsAny<object>()), Times.Exactly(2));
      }

      [Test]
      public void ReportModuleSubscriptionTest()
      {
         new ReportModuleUpdateReceiver(_history, _reportModuleInfo, _reportModuleNavigation);

         Assert.True(_history.Subscriptions.Any());
      }

      private SelectedItem SetUpSelectedItem(UniqueValueFactory uniqueValueFactory, Selection selection, Id targetId)
      {
         var selectedItem = new SelectedItem(uniqueValueFactory, selection.Id, targetId, 0);
         _history.Run(ss => AddSelected(ss, selectedItem));
         return selectedItem;
      }

      private ISnapShot AddSelected(ISnapShot snapShot, SelectedItem selected)
      { return snapShot.AddItem<SelectedItem>(selected); }

      private void SetUpReportModuleInfo(UniqueValueFactory uniqueValueFactory)
      {
         var moduleInfo = new NavigationModuleInfo(uniqueValueFactory, "Test", string.Empty, string.Empty, string.Empty, "R");
         var child = new NavigationModuleChildInfo(uniqueValueFactory, "ChildItem");

         Mock.Get(_reportModuleInfo).Setup(p => p.ModuleId).Returns(moduleInfo.Id);
         Mock.Get(_reportModuleInfo).Setup(p => p.ChildIds).Returns(new List<Id> { child.Id }.ToImmutableList());
      }
      [Test]
      public void UpdateShouldDeleteDeletedChilds()
      {
         // Arrange
         var navInfo = new NavigationModuleChildInfo(new Id<NavigationModuleChildInfo>(new UniqueValue(Guid.NewGuid())), "name");
         _history.Run(ss => AddNavModule(ss, navInfo));
         _history.Run(ss => DeleteNavModule(ss, navInfo));

         var reportUpdateReceiver = new ReportModuleUpdateReceiver(_history, _reportModuleInfo, _reportModuleNavigation);

         //Act
         reportUpdateReceiver.Update(_history.CurrentSnapShot);

         //Assert
         Assert.Zero(_history.CurrentSnapShot.GetItems<NavigationModuleChildInfo>().Count());
      }
      private ISnapShot AddNavModule(ISnapShot snapShot, NavigationModuleChildInfo nav)
      {
         return snapShot.AddItem(nav);
      }
      private ISnapShot DeleteNavModule(ISnapShot snapShot, NavigationModuleChildInfo nav)
      {
         return snapShot.DeleteItem(nav);
      }
   }
}