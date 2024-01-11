// <copyright file="StatusCellMergedStyleSelectorTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView;
using Moq;
using NUnit.Framework;
using Telerik.Windows.Controls.GridView;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.Table.Views.CustomGridView
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class StatusCellMergedStyleSelectorTest
   {
      [Test]
      public void SelectStyleShouldReturnStyle()
      {
         //arrenge
         var selector = new StatusCellMergedStyleSelector();

         //Act
         var ret = selector.SelectStyle(null, null);

         //Assert
         Assert.That(ret is Style);
      }
      [Test]
      public void SelectStyleWithMergedCellInfoPassShouldReturnStyle()
      {
         //arrenge
         var selector = new StatusCellMergedStyleSelector();
         var _cell = new Mock<MergedCellInfo>(new object[] { 0, 0, 0, 0, "Pass" });
         //Act
         var ret = selector.SelectStyle(_cell.Object, null);
         var setters = ret.Setters.GetEnumerator();
         setters.MoveNext();
         var current = (Setter)setters.Current;
         var color = new SolidColorBrush(Color.FromRgb(202, 231, 218)).ToString();
         //Assert
         Assert.That(current.Value.ToString() == color);
         //Assert
         Assert.That(ret is Style);
      }
      [Test]
      public void SelectStyleWithMergedCellInfoInvalidShouldReturnStyle()
      {
         //arrenge
         var selector = new StatusCellMergedStyleSelector();
         var _cell = new Mock<MergedCellInfo>(new object[] { 0, 0, 0, 0, "Invalid" });
         //Act
         var ret = selector.SelectStyle(_cell.Object, null);
         var setters = ret.Setters.GetEnumerator();
         setters.MoveNext();
         var current = (Setter)setters.Current;
         var color = new SolidColorBrush(Color.FromRgb(248, 210, 201)).ToString();
         //Assert
         Assert.That(current.Value.ToString() == color);
         //Assert
         Assert.That(ret is Style);
      }
      [Test]
      public void SelectStyleWithMergedCellInfoFailShouldReturnStyle()
      {
         //arrenge
         var selector = new StatusCellMergedStyleSelector();
         var _cell = new Mock<MergedCellInfo>(new object[] { 0, 0, 0, 0, "Fail" });
         //Act
         var ret = selector.SelectStyle(_cell.Object, null);
         var setters = ret.Setters.GetEnumerator();
         setters.MoveNext();
         var current = (Setter)setters.Current;
         var color = new SolidColorBrush(Color.FromRgb(248, 210, 201)).ToString();
         //Assert
         Assert.That(current.Value.ToString() == color);
      }
   }
}
