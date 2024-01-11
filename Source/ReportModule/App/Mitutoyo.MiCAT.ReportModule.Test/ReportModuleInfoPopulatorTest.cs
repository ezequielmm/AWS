// <copyright file="ReportModuleInfoPopulatorTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.ShellModule;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportModuleInfoPopulatorTest
   {
      private IAppStateHistory _history;
      private IReportModuleNavigation _reportModuleNavigation;

      [SetUp]
      public void Init()
      {
         _history = AppStateHelper.CreateAndInitializeAppStateHistory();
         _reportModuleNavigation = Mock.Of<IReportModuleNavigation>();
      }

      [Test]
      public void ReportModuleInfoPopulatorShouldAddModuleWithZeroOrdinalWhenNoOtherModulesArePresent()
      {
         //Act
         new ReportModuleInfoPopulator(_history, _reportModuleNavigation);
         //Assert
         var navigationListToModule = _history.CurrentSnapShot.GetItems<NavigationListToModule>().Single();
         Assert.AreEqual(new Ordinal(0), navigationListToModule.Order);
      }

      [Test]
      public void ReportModuleInfoPopulatorShouldAddModuleWithNextOrdinalWhenOtherModulesArePresent()
      {
         //Arrange
         var snapshot = _history.NextSnapShot(ControllerCall.Empty);
         var navigationList = snapshot.GetItems<NavigationList>().Single();
         var fakeModule = new NavigationModuleInfo(snapshot.NewUniqueValue(), "FakeModuleDisplayName", "FakeIconResourcePath", "FakeResourceKey", "FakeAssemblyName", "F");
         var navigationListToFakeModule = new NavigationListToModule(snapshot.NewUniqueValue(), navigationList.Id, fakeModule.Id, 5);
         snapshot = snapshot.AddItem(fakeModule).AddItem(navigationListToFakeModule);
         _history.AddSnapShot(snapshot);
         //Act
         new ReportModuleInfoPopulator(_history, _reportModuleNavigation);
         //Assert
         var navigationListToReportModule = _history.CurrentSnapShot.GetItems<NavigationListToModule>().Except(new[] { navigationListToFakeModule }).Single();
         Assert.AreEqual(navigationListToFakeModule.Order + 1, navigationListToReportModule.Order);
      }

      [Test]
      public void AddingChildToReportModule()
      {
         _ = new ReportModuleInfoPopulator(_history, _reportModuleNavigation);
         Assert.True(_history.CurrentSnapShot.GetItems<NavigationModuleInfo>().First().Keytip == "R");
      }

      [Test]
      public void AddingChildToReportModuleWithoutName()
      {
         var reportModulePopulator = new ReportModuleInfoPopulator(_history, _reportModuleNavigation);
         var uniqueValueFactory = new UniqueValueFactory();
         var module = new NavigationModuleInfo(uniqueValueFactory.UniqueValue(), "Report", "Img1", "Icon", "Report", "R");
         _history.Run(snapshot => snapshot.AddItem(module));
         var childInfo1 = reportModulePopulator.CreateChildItem();
         _history.Run(ss => AddItemHelper(ss, reportModulePopulator, module, childInfo1));

         var childInfo2 = reportModulePopulator.CreateChildItem();
         _history.Run(ss => AddItemHelper(ss, reportModulePopulator, module, childInfo2));
         Assert.AreEqual(reportModulePopulator.ChildIds.Count, 2);
      }

      private ISnapShot AddItemHelper(ISnapShot snapShot, ReportModuleInfoPopulator reportModulePopulator, NavigationModuleInfo moduleInfo, NavigationModuleChildInfo childInfo)
      {
        return reportModulePopulator.AddChildItems(moduleInfo, snapShot, childInfo);
      }

      [Test]
      public void ChildIdExistsShouldReturn()
      {
         //Arrange
         var reportModulePopulator = new ReportModuleInfoPopulator(_history, _reportModuleNavigation);
         var uniqueValueFactory = new UniqueValueFactory();
         var module = new NavigationModuleInfo(uniqueValueFactory.UniqueValue(), "Report", "Img1", "Icon", "Report", "R");
         _history.Run(snapshot => snapshot.AddItem(module));
         var childInfo1 = reportModulePopulator.CreateChildItem();
         _history.Run(ss => AddItemHelper(ss, reportModulePopulator, module, childInfo1));

         //Act
         var exist = reportModulePopulator.ChildIdExists(childInfo1.Id);

         //Assert
         Assert.IsTrue(exist);
      }
   }
}