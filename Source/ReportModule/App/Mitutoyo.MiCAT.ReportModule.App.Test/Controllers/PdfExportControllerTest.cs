// <copyright file="PdfExportControllerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Domain.Report;
using Mitutoyo.MiCAT.ReportModule.Persistence;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Providers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.App.Test.Controllers
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class PdfExportControllerTest
   {
      private Mock<IPdfPersistence> _persistence;
      private Mock<IPdfNameResolver> _nameResolver;
      private Mock<IPdfGenerator> _pdfGenerator;
      private Mock<IMessageNotifier> _messageNotifier;
      private IPdfExportController _controller;
      private Mock<IProcessLauncher> _processLauncher;

      [SetUp]
      public virtual void SetUp()
      {
         _persistence = new Mock<IPdfPersistence>();
         _nameResolver = new Mock<IPdfNameResolver>();
         _pdfGenerator = new Mock<IPdfGenerator>();
         _messageNotifier = new Mock<IMessageNotifier>();
         _processLauncher = new Mock<IProcessLauncher>();
         _controller = new PdfExportController(_persistence.Object, _nameResolver.Object, _pdfGenerator.Object, _messageNotifier.Object, _processLauncher.Object);
      }

      [Test]
      public void ExportReportShouldReturnTrue()
      {
         //Arrange
         var pdrResult = new PdfNameResult(new System.IO.FileInfo("test"), false);
         _nameResolver.Setup(r => r.QueryFileInfo()).Returns(pdrResult);

         _pdfGenerator.Setup(p => p.GetReportPdfAsync()).Returns(new ReportPdf(new byte[] { }));
         _persistence.Setup(p => p.SaveReport(It.IsAny<ReportPdf>(), It.IsAny<string>())).Returns(new Domain.DataResult.Result(Domain.DataResult.ResultState.Success));
         _processLauncher.Setup(p => p.LaunchProcess(It.IsAny<string>()));
         //Act
         var result = _controller.Export();

         //Assert
         Assert.True(result);
         _persistence.Verify(x => x.SaveReport(It.IsAny<ReportPdf>(), It.IsAny<string>()), Times.Once);
      }
      [Test]
      public void ExportReportShouldReturnFalseIfQueryNameIsCanceled ()
      {
         //Arrange
         var pdrResult = new PdfNameResult(null, true);
         _nameResolver.Setup(r => r.QueryFileInfo()).Returns(pdrResult);

         //Act
         var result = _controller.Export();

         //Assert
         Assert.False(result);
      }
      [Test]
      public void ExportReportShouldNotifyExceptionIfErrorOcurr()
      {
         //Arrange
         var pdrResult = new PdfNameResult(new System.IO.FileInfo("test"), false);
         _nameResolver.Setup(r => r.QueryFileInfo()).Returns(pdrResult);

         _pdfGenerator.Setup(p => p.GetReportPdfAsync()).Returns(new ReportPdf(new byte[] { }));
         _persistence.Setup(p => p.SaveReport(It.IsAny<ReportPdf>(), It.IsAny<string>())).Returns(new Domain.DataResult.Result(Domain.DataResult.ResultState.Error));
         _processLauncher.Setup(p => p.LaunchProcess(It.IsAny<string>()));
         //Act
         var result = _controller.Export();

         //Assert
         Assert.False(result);
         _messageNotifier.Verify(x => x.NotifyError(It.IsAny<ResultException>()), Times.Once);
      }
   }
}
