// <copyright file="VMReportTemplateConfirmationDialogTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog.Confirmation;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Dialog.Confirmation
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMReportTemplateConfirmationDialogTest
   {
      [Test]
      public void VMReportTemplateDeleteConfirmationDialogShouldConstructVMConfirmationDialog()
      {
         //Act
         var dialog = new VMReportTemplateDeleteConfirmationDialog();
         //Assertion
         Assert.IsTrue(dialog is VMConfirmationDialog);
      }
   }
}
