// <copyright file="ShellCloseServiceTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.ReportModuleManager;
using Mitutoyo.MiCAT.ReportModule.Services;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Mitutoyo.MiCAT.ShellModule;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Test.Utilities
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ShellCloseServiceTest
   {
      private Mock<IReportModuleCloseManager> _reportModuleCloseManager;
      private ICloseService _closeService;
      [SetUp]
      public void Setup()
      {
         _reportModuleCloseManager = new Mock<IReportModuleCloseManager>();
         var instanceGuid = Guid.NewGuid();
         _closeService = new ShellCloseService(_reportModuleCloseManager.Object, new Id<NavigationModuleChildInfo>(instanceGuid));
      }
      [Test]
      public void CloseInstanceShouldCallReportModuleCloseManager()
      {
         //Act
         _closeService.CloseInstance();
         //Assert
         _reportModuleCloseManager.Verify(d => d.CloseWorkspaceInstanceById(It.IsAny<Id<NavigationModuleChildInfo>>()),Times.Once);
      }
   }
}
