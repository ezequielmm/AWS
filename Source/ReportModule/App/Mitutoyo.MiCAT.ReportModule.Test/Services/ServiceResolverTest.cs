// <copyright file="ServiceResolverTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Common.Help;
using Mitutoyo.MiCAT.ReportModule.ReportModuleManager;
using Mitutoyo.MiCAT.ReportModule.Services;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Mitutoyo.MiCAT.ShellModule;
using Moq;
using NUnit.Framework;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule.Test.Services
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ServiceResolverTest
   {
      private IUnityContainer _unityContainer;
      [SetUp]
      public void Setup()
      {
         _unityContainer = new UnityContainer();
         _unityContainer.RegisterInstance(Mock.Of<IAppStateHistory>());
         _unityContainer.RegisterInstance(Mock.Of<IHelpTopicController>());
         _unityContainer.RegisterInstance(Mock.Of<IReportModuleCloseManager>());
      }
      [Test]
      public void ResolveCloseServiceFromShellContainerReturnCloseService()
      {
         var instanceGuid = Guid.NewGuid();
         //Act
         var closeService = ServicesResolver.ResolveCloseServiceFromShellContainer(_unityContainer, new Id<NavigationModuleChildInfo>(instanceGuid));
         //Assert
         Assert.That(closeService is ICloseService);
      }
            [Test]
      public void ResolveCloseServiceFromShellContainerReturnHelpService()
      {
         //Act
         var closeService = ServicesResolver.ResolveHelpServiceFromShellContainer(_unityContainer);
         //Assert
         Assert.That(closeService is IHelpService);
      }
   }
}
