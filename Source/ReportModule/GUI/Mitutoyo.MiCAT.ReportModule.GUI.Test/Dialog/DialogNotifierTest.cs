// <copyright file="DialogNotifierTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Dialog
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class DialogNotifierTest
   {
      private Mock<IDialogService> _dialogService;
      [SetUp]
      public void Setup()
      {
         _dialogService = new Mock<IDialogService>();
      }
      [Test]
      public void NotifyErrorShouldCallShowError()
      {
         //Arregment
         var dialogNotifier = new DialogNotifier(_dialogService.Object);

         //Act
         dialogNotifier.NotifyError(new ResultException("Error","ErrorKey"));

         //Assert
         _dialogService.Verify(d => d.ShowError(It.IsAny<Exception>()), Times.Once);
      }
   }
}
