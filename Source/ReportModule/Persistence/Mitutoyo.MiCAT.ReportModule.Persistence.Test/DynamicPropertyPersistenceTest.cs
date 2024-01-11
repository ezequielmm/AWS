// <copyright file="DynamicPropertyPersistenceTest.cs" company="Mitutoyo Europe GmbH">
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
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
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
   public class DynamicPropertyPersistenceTest
   {
      private IMapper _mapper;
      private Mock<IDataServiceClient> _dataServiceClient;
      private Mock<IAssemblyConfigurationHelper> _connectionStringProvider;
      private List<PropertyDefinitionDTO> _propertyDefinitionResults;
      [SetUp]
      public virtual void SetUp()
      {
         _mapper = AutoMapperConfig.InitializeAutoMapper().CreateMapper();
         _dataServiceClient = new Mock<IDataServiceClient>();
         _connectionStringProvider = new Mock<IAssemblyConfigurationHelper>();

         _propertyDefinitionResults = new List<PropertyDefinitionDTO>()
         {
            new PropertyDefinitionDTO()
            {
               Id = Guid.NewGuid(),
               DisplayName = "Property Definition 1"
            }
         };

         DataServiceResult<ServiceInfoDTO> result =
            new DataServiceResult<ServiceInfoDTO>(new HttpResponseMessage(HttpStatusCode.InternalServerError),
               new DataServiceError(null));

         _dataServiceClient.Setup(x => x.GetServiceInfo()).Returns(Task.Run(() => result));

         var pdResults =
            new DataServiceResult<IEnumerable<PropertyDefinitionDTO>>(new HttpResponseMessage(HttpStatusCode.OK), _propertyDefinitionResults);
         _dataServiceClient.Setup(x => x.GetPropertyDefinitions(Web.Data.EntityType.MeasurementResult)).Returns(Task.Run(() => pdResults));
      }

      [Test]
      public async Task GetDynamicProperty()
      {
         // Arrange
         var service = new Mock<IPersistenceDataService>();

         service.Setup(s => s.GetPropertyDefinitions(Web.Data.EntityType.MeasurementResult))
            .Returns(Task.FromResult(new SuccessResult<IEnumerable<PropertyDefinitionDTO>>(new[] { new PropertyDefinitionDTO() }) as Result));

         Mock<IPersistenceServiceLocator> persistenceServiceLocator = new Mock<IPersistenceServiceLocator>();
         persistenceServiceLocator
            .Setup(x => x.GetService<IPersistenceDataService>(Location.DataService, Mechanism.DataService))
            .Returns(service.Object);

         var pdPersistence = new DynamicPropertyPersistence(persistenceServiceLocator.Object, _mapper);

         // Act
         var result = await pdPersistence.GetDynamicProperties();

         // Assert
         Assert.AreEqual(result.Count(), 1);
      }
   }
}
