// <copyright file="WorkspaceContainerResolverTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.ReportModuleManager;
using Mitutoyo.MiCAT.ReportModuleApp.Manager;
using Moq;
using NUnit.Framework;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule.Test.ReportModuleManager
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class WorkspaceContainerResolverTest
   {
      private Mock<IReportModuleNavigation> _reportModuleNavigationMock;
      private IUnityContainer _unityContainer;
      private Mock<IWorkspaceCloseManager> _workspaceCloseManagerMock;
      [SetUp]
      public void Setup()
      {
         _reportModuleNavigationMock = new Mock<IReportModuleNavigation>();
         _workspaceCloseManagerMock = new Mock<IWorkspaceCloseManager>();
         _unityContainer = new UnityContainer();
         _unityContainer.RegisterInstance(Mock.Of<IWorkspaceCloseManager>());
      }
      [Test]
      public void GetWorkspaceControllerShouldReturnGetWorkspaceCloseController()
      {
         //Arrenge
         var id = Id.Empty;
         var containerResolver = new WorskpaceContainerResolver(_reportModuleNavigationMock.Object);
         _reportModuleNavigationMock.Setup(r => r.GetContainer(It.IsAny<Id>())).Returns(_unityContainer);
         //Act
         var workspaceCloseManager = containerResolver.GetWorkspaceController(id);
         //Assert
         Assert.That(workspaceCloseManager is IWorkspaceCloseManager);
      }
   }
}
