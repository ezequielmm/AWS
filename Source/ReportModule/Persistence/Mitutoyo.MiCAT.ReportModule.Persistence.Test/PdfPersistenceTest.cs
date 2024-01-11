// <copyright file="PdfPersistenceTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.Domain.Report;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class PdfPersistenceTest
   {
      private PdfPersistence _service;
      private Mock<IPersistenceServiceLocator> persistenceServiceLocator;

      [SetUp]
      public virtual void SetUp()
      {
         persistenceServiceLocator = new Mock<IPersistenceServiceLocator>();
         persistenceServiceLocator
            .Setup(x => x.GetService<IFilePersistenceService<ReportPdf>>(Location.File, Mechanism.Pdf))
            .Returns(new PdfPersistenceService());

         _service = new PdfPersistence(persistenceServiceLocator.Object);
      }

      [Test]
      public void SaveTemplateWithoutFileNameShouldReturnAnError()
      {
         var result = _service.SaveReport(CreateReporPdfByDefault(), null);
         Assert.IsTrue(result.IsError);
      }

      [Test]
      public void SaveTemplateWithoutAReportPdfShouldReturnAnError()
      {
         var result = _service.SaveReport(null, SetFileNameByDefault());
         Assert.IsTrue(result.IsError);
      }

      [Test]
      public void SaveReportWithFileAndFilePathShouldReturnIsSuccess()
      {
         var result = _service.SaveReport(CreateReporPdfByDefault(), SetFileNameByDefault());
         Assert.IsTrue(result.IsSuccess);
      }

      private static ReportPdf CreateReporPdfByDefault()
      {
         var bytes = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
         return new ReportPdf(bytes);
      }

      private string SetFileNameByDefault()
      {
         return AppDomain.CurrentDomain.BaseDirectory + "/" + Guid.NewGuid() + ".pdf";
      }
   }
   }
