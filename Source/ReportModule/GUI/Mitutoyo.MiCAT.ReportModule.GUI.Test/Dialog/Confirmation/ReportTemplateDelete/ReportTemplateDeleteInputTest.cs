// <copyright file="ReportTemplateDeleteInputTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog.Confirmation;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog.Confirmation.ReportTemplateDelete;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Dialog.Confirmation.ReportTemplateDelete
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportTemplateDeleteInputTest
   {
      private Mock<IDialogService> _dialogServiceMock;
      [SetUp]
      public void Setup()
      {
         _dialogServiceMock = new Mock<IDialogService>();
      }
      [Test]
      public void ConfirmDeleteReportTemplateShouldReturnTrue()
      {
         _dialogServiceMock.Setup(d => d.ShowDialog(It.IsAny<VMConfirmationDialog>(),It.IsAny<DialogType>()))
            .Callback((VMConfirmationDialog vm, DialogType type)=> { vm.CanContinue = true; });
         //Act
         var reportTemplateDeleteInput = new ReportTemplateDeleteInput(_dialogServiceMock.Object);
         var ret = reportTemplateDeleteInput.ConfirmDeleteReportTemplate();
         //Assertion
         Assert.IsTrue(ret);
      }
   }
}
