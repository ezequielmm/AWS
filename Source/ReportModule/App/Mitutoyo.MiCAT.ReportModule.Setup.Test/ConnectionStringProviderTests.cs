// <copyright file="ConnectionStringProviderTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service.Helpers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Test
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class ConnectionStringProviderTests
   {
      [Test]
      public void GetDataServiceApiUrl_ShouldReturnDefaultUrl()
      {
         // Arrange
         var assembly = new Mock<Assembly>();
         assembly.SetupGet(x => x.Location).Returns(String.Empty);
         var service = new AssemblyConfigurationHelper(assembly.Object);
         var defaultValue = "http://localhost:48254/";
         // Act
         var dataServiceApiUrl = service.GetDataServiceApiUrl();
         // Assert
         Assert.AreEqual(dataServiceApiUrl, defaultValue);
      }
   }
}