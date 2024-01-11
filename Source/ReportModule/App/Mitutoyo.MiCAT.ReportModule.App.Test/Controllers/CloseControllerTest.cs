// <copyright file="CloseControllerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.App.Test.Controllers
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class CloseControllerTest
   {
      private ICloseController _closeController;
      private Mock<ICloseService> _closeService;
      private Guid _instanceId;
      [SetUp]
      public void Setup()
      {
         _instanceId = Guid.NewGuid();
         _closeService = new Mock<ICloseService>();
         _closeController = new CloseController(_closeService.Object);
      }
      [Test]
      public void CloseWorskpaceShouldCallReportModuleCloseManager()
      {
         //Act
         _closeController.CloseWorkspace();
         //Assert
         _closeService.Verify(c => c.CloseInstance(), Times.Once);
      }
   }
}
