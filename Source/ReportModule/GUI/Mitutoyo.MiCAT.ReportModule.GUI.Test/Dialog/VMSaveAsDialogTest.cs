// <copyright file="VMSaveAsDialogTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog;
using Mitutoyo.MiCAT.ReportModule.GUI.Dialog.SaveAs;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Dialog
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMSaveAsDialogTest
   {
      [Test]
      public void VMSaveAsDialogTest_CanSaveShouldBeTrue()
      {
         //Arrange
         var sut = new VMSaveAsDialog();

         //Act
         sut.Name = "My Template";

         //Assert
         Assert.IsTrue(sut.CanSave);
      }

      [Test]
      public void VMSaveAsDialogTest_CanSaveShouldBeFalseWhenNameEmpty()
      {
         //Arrange
         var sut = new VMSaveAsDialog();

         //Act
         var ex = Assert.Throws(typeof(ValidationException), () => { sut.Name = string.Empty; });

         //Assert
         Assert.IsFalse(sut.CanSave);
      }

      [Test]
      public void VMSaveAsDialogTest_CanSaveShouldBeFalseWhenNameExceedsLength()
      {
         //Arrange
         var sut = new VMSaveAsDialog();
         var name = new string('T', 251);

         //Act
         var ex = Assert.Throws(typeof(ValidationException), () => { sut.Name = name; });

         //Assert
         Assert.IsFalse(sut.CanSave);
      }

      [Test]
      public void VMSaveAsDialogTest_SaveCommandShouldSetOkResult()
      {
         //Arrange
         var sut = new VMSaveAsDialog();
         sut.Name = "TemplateName";

         //Act
         sut.SaveCommand.Execute(null);

         //Assert
         Assert.AreEqual(DialogResults.Ok, sut.DialogResult);
      }

      [Test]
      public void VMSaveAsDialogTest_SaveCommandShouldNotSetOkResult()
      {
         //Arrange
         var sut = new VMSaveAsDialog();
         sut.CanSave = false;

         //Act
         sut.SaveCommand.Execute(null);

         //Assert
         Assert.AreEqual(DialogResults.None, sut.DialogResult);
      }

      [Test]
      public void VMSaveAsDialogTest_SaveCommandShouldSetCancelResult()
      {
         //Arrange
         var sut = new VMSaveAsDialog();
         sut.Name = "TemplateName";

         //Act
         sut.CancelCommand.Execute(null);

         //Assert
         Assert.AreEqual(DialogResults.Canceled, sut.DialogResult);
      }
   }
}
