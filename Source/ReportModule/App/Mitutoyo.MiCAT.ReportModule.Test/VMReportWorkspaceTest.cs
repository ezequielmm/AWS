// <copyright file="VMReportWorkspaceTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Controllers;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.SelectTemplates;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ShellModule;
using Moq;
using NUnit.Framework;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMReportWorkspaceTest
   {
      private IAppStateHistory _history;
      private IReportModuleInfo _reportModulePopulator;
      private IReportModuleNavigation _reportModuleNavigation;
      private IReportTemplateController _reportTemplateController;
      private ISnapShot _snapShot;
      private IUnityContainer _container;

      [SetUp]
      public void Init()
      {
         _history = Mock.Of<IAppStateHistory>();
         _snapShot = Mock.Of<ISnapShot>();
         _reportModulePopulator = Mock.Of<IReportModuleInfo>();
         _reportTemplateController = Mock.Of<IReportTemplateController>();
         _reportModuleNavigation = Mock.Of<IReportModuleNavigation>();
         _container = new UnityContainer();
         _container.RegisterInstance(_history);
         _container.RegisterInstance(_reportTemplateController);
      }

      [Test]
      public void AddingNewReport()
      {
         var uniqueValueFactory = new UniqueValueFactory();
         Mock.Get(_history).Setup(a => a.NextSnapShot(It.IsAny<ControllerCall>())).Returns(_snapShot);
         var module = new NavigationModuleInfo(uniqueValueFactory.UniqueValue(), "Report", "Img1", "ReportWorkspace_iconDrawingImage", "Report", "R");
         Mock.Get(_snapShot).Setup(s => s.GetItems<NavigationModuleInfo>())
            .Returns(new List<NavigationModuleInfo>{module});
         Mock.Get(_reportModulePopulator).Setup(s => s.CreateChildItem())
            .Returns(new NavigationModuleChildInfo(It.IsAny<Guid>(), It.IsAny<string>()));

         var vmSelectTemplate = new VMSelectTemplateForCreate(_reportTemplateController);
         _container.RegisterInstance(vmSelectTemplate);
         Mock.Get(_reportModuleNavigation).Setup(m => m.CreateContainerForWorkspace(It.IsAny<Id<NavigationModuleChildInfo>>()))
            .Returns(_container);
         var template = new ReportTemplate();
         Mock.Get(_reportTemplateController).Setup(x => x.GetReportTemplateById(It.IsAny<Guid>()))
            .Returns(Task.FromResult(template));

         var sut = new VMReportWorkspace(_history, _reportModulePopulator, _reportModuleNavigation);
         sut.CreateReportCommand.Execute(null);
         Mock.Get(_reportTemplateController).Verify(r=>r.GetReportTemplateDescriptorsForCreate(),Times.Once);
      }
   }
}
