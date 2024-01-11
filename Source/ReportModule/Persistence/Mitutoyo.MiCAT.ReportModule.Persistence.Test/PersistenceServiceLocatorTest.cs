// <copyright file="PersistenceServiceLocatorTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Mitutoyo.MiCAT.DataServiceClient;
using Mitutoyo.MiCAT.ReportModule.Domain.Report;
using Mitutoyo.MiCAT.ReportModule.Persistence.Components;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service.Helpers;
using Moq;
using NUnit.Framework;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class PersistenceServiceLocatorTest
   {
      public IPersistenceServiceLocator _serviceLocator;
      //public IIOCContainer _iOCContainer;
      public IUnityContainer _iOCContainer;

      [SetUp]
      public virtual void SetUp()
      {
         _iOCContainer = new UnityContainer();
         _iOCContainer.RegisterInstance(new Mock<IDataServiceClient>().Object);
         _iOCContainer.RegisterInstance(new Mock<IMapper>().Object);
         _iOCContainer.RegisterInstance(new Mock<IAssemblyConfigurationHelper>().Object);
         _serviceLocator = new PersistenceServiceLocator(_iOCContainer);
      }

      [Test]
      public void LocationFileAndMechanismPdfShouldReturnPdfPersistenceService()
      {
         var service = _serviceLocator.GetService<IFilePersistenceService<ReportPdf>>(Location.File, Mechanism.Pdf);
         Assert.AreEqual(service.GetType(), typeof(PdfPersistenceService));
      }

      [Test]
      public void LocationFileAndMechanismXmlShouldReturnXmlPersistenceServiceForReportTemplateSO()
      {
         var service = _serviceLocator.GetService<IFilePersistenceService<ReportTemplateSO>>(Location.File, Mechanism.Xml);
         Assert.AreEqual(service.GetType(), typeof(XmlPersistenceService<ReportTemplateSO>));
      }
      [Test]
      public void LocationDataServiceAndMechanismDataServiceShouldReturnPersistenDataService()
      {
         var service = _serviceLocator.GetService<IPersistenceDataService>(Location.DataService, Mechanism.DataService);
         Assert.AreEqual(service.GetType().GetInterface("IPersistenceDataService"), typeof(IPersistenceDataService));
      }
   }
}
