// <copyright file="StatusCellStyleSelectorTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.Table.Views.CustomGridView
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class StatusCellStyleSelectorTest
   {
      [Test]
      [TestCase("Pass", "#FFCAE7DA")]
      [TestCase("Fail", "#FFF8D2C9")]
      [TestCase("Invalid", "#FFF8D2C9")]
      [TestCase("UnknownValue", "#FFFFFFFF")]
      public void SelectStyleShouldReturnStyle(string evaluatedCharacteristicStatus, string resultColor)
      {
         //arrenge
         var selector = new StatusCellStyleSelector();
         var evaluatedCharacteristic = new EvaluatedCharacteristic(new Characteristic(), new CharacteristicActual(Guid.NewGuid(), "aver", null, null));
         var vmEvaluatedCharacteristic = new VMEvaluatedCharacteristic(evaluatedCharacteristic);
         vmEvaluatedCharacteristic.Status = evaluatedCharacteristicStatus;

         //Act
         var ret = selector.SelectStyle(vmEvaluatedCharacteristic, null);

         //Assert
         Assert.That(ret is Style);

         Assert.AreEqual(1, ret.Setters.Count);
         Setter setter = ret.Setters[0] as Setter;

         Assert.AreEqual("Background", setter.Property.ToString());
         Assert.AreEqual(resultColor, setter.Value.ToString());
      }
   }
}
