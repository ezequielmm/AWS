// <copyright file="ReportModuleTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Input;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.ShellModule;
using Moq;
using NUnit.Framework;
using Prism.Regions;
using Prism.Unity.Ioc;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportModuleTest
   {
      private IRegionManager _regionManager;
      private IUnityContainer _unityContainer;
      private ReportModule _reportModule;

      [SetUp]
      public void Setup()
      {
         _regionManager = Mock.Of<IRegionManager>();
         _unityContainer = new UnityContainer();
         _unityContainer.RegisterInstance(_regionManager);
         _reportModule = new ReportModule();
      }

      [Test]
      public void ModuleInitializeTest()
      {
         var uniqueValueFactory = new UniqueValueFactory();
         var appstateHistory = Mock.Of<IAppStateHistory>();
         var globalCommands = Mock.Of<IGlobalCommands>();
         var moduleInfo = Mock.Of<IReportModuleInfo>();
         var snapShot = Mock.Of<ISnapShot>();
         var nl = new NavigationList(uniqueValueFactory.UniqueValue());

         Mock.Get(snapShot).Setup(s => s.GetItems<NavigationList>()).Returns(new List<NavigationList> { nl });
         Mock.Get(snapShot).Setup(s => s.AddItem(It.IsAny<NavigationModuleInfo>())).Callback(() => { }).Returns(snapShot);
         Mock.Get(snapShot).Setup(s => s.AddItem(It.IsAny<NavigationListToModule>())).Callback(() => { }).Returns(snapShot);
         Mock.Get(snapShot).Setup(s => s.AddItem(It.IsAny<NavigationModuleChildInfo>())).Callback(() => { }).Returns(snapShot);
         Mock.Get(snapShot).Setup(s => s.AddItem(It.IsAny<NavigationModuleToChild>())).Callback(() => { }).Returns(snapShot);

         Mock.Get(appstateHistory).Setup(a => a.NextSnapShot(It.IsAny<ControllerCall>())).Returns(snapShot);
         Mock.Get(appstateHistory).Setup(a => a.UniqueValueFactory).Returns(uniqueValueFactory);

         Mock.Get(_regionManager).Setup(r => r.Regions[It.IsAny<string>()].GetView(It.IsAny<string>())).Returns(new object());
         Mock.Get(_regionManager).Setup(r => r.Regions[It.IsAny<string>()].Add(It.IsAny<object>(), It.IsAny<string>())).Callback(() => { });
         Mock.Get(_regionManager).Setup(r => r.Regions[It.IsAny<string>()].ActiveViews).Returns(new ViewsCollection(new ObservableCollection<ItemMetadata>(), metadata => false));

         Mock.Get(globalCommands).Setup(g => g.InstanceCloseCommand.RegisterCommand(Mock.Of<ICommand>()));
         Mock.Get(globalCommands).Setup(g => g.ExitCommand.RegisterCommand(Mock.Of<ICommand>()));

         _unityContainer.RegisterInstance(appstateHistory);
         _unityContainer.RegisterInstance(globalCommands);
         _unityContainer.RegisterInstance(moduleInfo);

         var prismContainer = new UnityContainerExtension(_unityContainer);
         _reportModule.RegisterTypes(prismContainer);
         _reportModule.OnInitialized(prismContainer);
         Assert.IsTrue(_unityContainer.Registrations.Any());
      }
   }
}
