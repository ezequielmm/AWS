// <copyright file="PdfPersistenceServiceTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.Domain.Report;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Service.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class PdfPersistenceServiceTest
   {
      [Test]
      public void Save_ThrowException_ReportWithoutData()
      {
         // Arrange
         string fileName = "example.pdf";

         var service = new PdfPersistenceService();

         // Act & Assert
         var ex = Assert.Throws<ArgumentNullException>(() => service.Save(null, fileName));
         Assert.That(ex.ParamName, Is.EqualTo("Report to save cannot be null"));
      }

      [Test]
      public void Save_ThrowException_ReportWithoutName()
      {
         // Arrange
         string fileName = string.Empty;
         ReportPdf data = new ReportPdf(new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 });

         var service = new PdfPersistenceService();

         // Act & Assert
         var ex = Assert.Throws<ArgumentNullException>(() => service.Save(data, fileName));
         Assert.That(ex.ParamName, Is.EqualTo("File name cannot be null"));
      }

      [Test]
      public void Load_ThrowException_NotImplemented()
      {
         var service = new PdfPersistenceService();

         // Act & Assert
         Assert.Throws<NotImplementedException>(() => service.Load(string.Empty));
      }
   }
}
