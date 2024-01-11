// <copyright file="VMErrorDialogTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModule.GUI.Strings;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Dialog
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMErrorDialogTest
   {
      [Test]
      public void VMErrorDialogConstructorTest()
      {
         // Arrange:
         string title = "Title to test.";
         string message = "Message to test.";

         // Act:
         VMErrorDialog vmToTest = new VMErrorDialog(title, message);

         // Assert:
         Assert.AreEqual(title, vmToTest.Title);
         Assert.AreEqual(message, vmToTest.Message);
         Assert.IsNotNull(vmToTest.CloseCommand);
         Assert.AreEqual(Resources.ResourceManager.GetString("OkButtonLabel"), vmToTest.CloseButtonLabel);
      }

      [Test]
      public void VMErrorDialogFromCastingExeptionTest()
      {
         // Arrange:
         ResultException exceptionToCast = new ResultException("ExceptionKey");

         // Act:
         VMErrorDialog vmToTest = (VMErrorDialog)exceptionToCast;

         // Assert:
         Assert.AreEqual(StringFinder.FindLocalizedString(exceptionToCast.Key + "Title"), vmToTest.Title);
         Assert.AreEqual(StringFinder.FindLocalizedString(exceptionToCast.Key + "Message"), vmToTest.Message);
      }
   }
}
