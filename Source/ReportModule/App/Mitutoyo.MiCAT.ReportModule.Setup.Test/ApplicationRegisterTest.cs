// <copyright file="ApplicationRegisterTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar.MarginsSettings;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.StatusBar;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.Zoom;
using Mitutoyo.MiCAT.ReportModule.GUI.Navigation;
using Mitutoyo.MiCAT.Utilities.IoC;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ApplicationRegisterTest
   {
      private Mock<IServiceRegistrar> serviceRegistrarMock;
      private Mock<IServiceLocator> serviceLocatorMock;
      private ApplicationRegister sut;

      [SetUp]
      public void Setup()
      {
         // Arrange
         serviceRegistrarMock = new Mock<IServiceRegistrar> { DefaultValue = DefaultValue.Mock };
         serviceRegistrarMock.SetReturnsDefault(serviceRegistrarMock.Object);

         serviceLocatorMock = new Mock<IServiceLocator>();
         var commonRegistrarMock = new Mock<ICommonRegistrar>();

         sut = new ApplicationRegister(serviceRegistrarMock.Object, serviceLocatorMock.Object, commonRegistrarMock.Object);
      }

      [Test]
      public void ViewModelsShouldBeRegistered()
      {
         // Act
         sut.Registration();

         // Assert
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<VMParts>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<VMRuns>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<VMReportViewWorkspace>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<VMPageSizeSettings>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<VMMarginSizeSettings>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<IVMZoomFactor, VMZoomFactor>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<IVMPageNumberInfo, VMPageNumberInfo>(), Times.Once);
         serviceRegistrarMock.Verify(s => s.RegisterSingleton<IReportModeProperty, ReportModeProperty>(), Times.Once);
      }
   }
}
