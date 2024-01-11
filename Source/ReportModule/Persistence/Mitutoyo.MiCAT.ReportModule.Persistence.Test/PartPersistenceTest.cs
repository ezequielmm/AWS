// <copyright file="PartPersistenceTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Mitutoyo.MiCAT.DataServiceClient;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service.Helpers;
using Mitutoyo.MiCAT.ReportModule.Setup.Configurations;
using Mitutoyo.MiCAT.Web.Data;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class PartPersistenceTest
   {
      private IMapper _mapper;
      private Mock<IDataServiceClient> _dataServiceClient;
      private List<PartDTO> _partsResults;
      private Guid _id;
      private Mock<IAssemblyConfigurationHelper> _connectionStringProvider;

      [SetUp]
      public virtual void SetUp()
      {
         _mapper = AutoMapperConfig.InitializeAutoMapper().CreateMapper();
         _dataServiceClient = new Mock<IDataServiceClient>();
         _connectionStringProvider = new Mock<IAssemblyConfigurationHelper>();

         _id = Guid.NewGuid();
         var partResultId2 = Guid.NewGuid();
         var partResultId3 = Guid.NewGuid();

         _partsResults = new List<PartDTO>()
         {
            new PartDTO()
            {
               Id = _id,
               Name = "PartDescriptor 1"
            },
            new PartDTO()
            {
               Id = partResultId2,
               Name = "PartDescriptor 2"
            },
            new PartDTO()
            {
               Id = partResultId3,
               Name = "PartDescriptor 3"
            }
         };

         DataServiceResult<ServiceInfoDTO> result =
            new DataServiceResult<ServiceInfoDTO>(
               new HttpResponseMessage(HttpStatusCode.OK),
               new ServiceInfoDTO());

         _dataServiceClient.Setup(x => x.GetServiceInfo()).Returns(Task.FromResult(result));

         var partResults =
            new DataServiceResult<IEnumerable<PartDTO>>(new HttpResponseMessage(HttpStatusCode.OK), _partsResults);
         _dataServiceClient.Setup(x => x.GetAllParts()).Returns(Task.FromResult(partResults));
      }

      [Test]
      public async Task GetPartsShouldReturnParts()
      {
         // Arrange
         var service = new PersistenceDataService(_dataServiceClient.Object);

         Mock<IPersistenceServiceLocator> persistenceServiceLocator = new Mock<IPersistenceServiceLocator>();
         persistenceServiceLocator
            .Setup(x => x.GetService<IPersistenceDataService>(Location.DataService, Mechanism.DataService))
            .Returns(service);

         var partPersistence = new PartPersistence(persistenceServiceLocator.Object, _mapper);

         // Act
         var result = await partPersistence.GetParts();

         // Assert
         Assert.AreEqual(result.Count(), 3);
         Assert.AreEqual(result.FirstOrDefault().Id, _id);
         Assert.AreEqual(result.FirstOrDefault().Name, "PartDescriptor 1");
      }
   }
}
