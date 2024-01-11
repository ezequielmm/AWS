// <copyright file="DataServiceClientConfiguratorTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.DataServiceClient;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service.Helpers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Service.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class DataServiceClientConfiguratorTest
   {
      private Mock<IAssemblyConfigurationHelper> _assemblyConfigurationHelper;
      private Mock<IDataServiceClient> _dataServiceClient;

      [SetUp]
      public virtual void SetUp()
      {
         _assemblyConfigurationHelper = new Mock<IAssemblyConfigurationHelper>();
         _dataServiceClient = new Mock<IDataServiceClient>();
      }

      [Test]
      public void Configure_Test()
      {
         // Arrange
         const string URI = "https://www.test.com";
         var dataServiceClientConfigurator = new DataServiceClientConfigurator(_dataServiceClient.Object, _assemblyConfigurationHelper.Object);

         _assemblyConfigurationHelper.Setup(x => x.GetDataServiceApiUrl()).Returns(URI);

         // Act
         dataServiceClientConfigurator.Configure();

         // Assert
         _dataServiceClient.Verify(x => x.SetDataServiceUri(URI));
      }
   }
}
