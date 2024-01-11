// <copyright file="VMSelectDistinctFilterControlTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter;
using NUnit.Framework;
using static Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter.VMSelectDistinctFilterControl;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.Table.Views.CustomGridView
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMSelectDistinctFilterControlTest
   {
      [Test]
      public void VMSelectDistinctFilterControl_ShouldBeNotFilteredAndAllOptionsShouldBeDisplayedByDefault()
      {
         var options = new[]
         {
            new SelectDistinctFilterOption { Name = "Opt1", Value = "Opt1" },
            new SelectDistinctFilterOption { Name = "Opt2", Value = "Opt2" },
         };
         var sut = new VMSelectDistinctFilterControl(options);

         Assert.IsFalse(sut.IsEmpty);
         Assert.AreEqual(options.Count(), sut.DistinctOptions.View.Cast<SelectDistinctFilterOption>().Count());
      }

      [Test]
      public void VMSelectDistinctFilterControl_WithoutOptionsShouldBeEmpty()
      {
         var sut = new VMSelectDistinctFilterControl(Array.Empty<SelectDistinctFilterOption>());

         Assert.IsTrue(sut.IsEmpty);
      }

      [Test]
      public void VMSelectDistinctFilterControl_WithOptionsShouldNotBeEmpty()
      {
         var options = new[]
        {
            new SelectDistinctFilterOption { Name = "Opt1", Value = "Opt1", IsChecked = true, },
            new SelectDistinctFilterOption { Name = "Opt2", Value = "Opt2", IsChecked = true, },
         };
         var sut = new VMSelectDistinctFilterControl(options);

         Assert.IsFalse(sut.IsEmpty);
      }

      [Test]
      public void VMSelectDistinctFilterControl_IsAllSelectedShouldBeTrue()
      {
         var options = new[]
         {
            new SelectDistinctFilterOption { Name = "Opt1", Value = "Opt1", IsChecked = true, },
            new SelectDistinctFilterOption { Name = "Opt2", Value = "Opt2", IsChecked = true, },
         };
         var sut = new VMSelectDistinctFilterControl(options);

         Assert.IsTrue(sut.IsAllSelected);
      }

      [Test]
      public void VMSelectDistinctFilterControl_HasSomeSelectedShouldBeFalse()
      {
         var options = new[]
         {
            new SelectDistinctFilterOption { Name = "Opt1", Value = "Opt1", IsChecked = false, },
            new SelectDistinctFilterOption { Name = "Opt2", Value = "Opt2", IsChecked = false, },
         };
         var sut = new VMSelectDistinctFilterControl(options);

         Assert.IsFalse(sut.HasSomeSelected);
      }

      [Test]
      public void VMSelectDistinctFilterControl_HasSomeSelectedShouldBeTrue()
      {
         var options = new[]
         {
            new SelectDistinctFilterOption { Name = "Opt1", Value = "Opt1", IsChecked = true, },
            new SelectDistinctFilterOption { Name = "Opt2", Value = "Opt2", IsChecked = false, },
         };
         var sut = new VMSelectDistinctFilterControl(options);

         Assert.IsTrue(sut.HasSomeSelected);
      }

      [Test]
      public void VMSelectDistinctFilterControl_ClearAllWithAllSelectedShouldUnSelectAllOptions()
      {
         var options = new[]
         {
            new SelectDistinctFilterOption { Name = "Opt1", Value = "Opt1", IsChecked = true, },
            new SelectDistinctFilterOption { Name = "Opt2", Value = "Opt2", IsChecked = true, },
         };
         var sut = new VMSelectDistinctFilterControl(options);

         sut.ClearFilterCommand.Execute(null);

         Assert.IsFalse(sut.IsAllSelected);
         Assert.IsFalse(sut.HasSomeSelected);
         Assert.IsTrue(options.All(x => x.IsChecked == false));
      }

      [Test]
      public void VMSelectDistinctFilterControl_ClearAllShouldUnSelectAllOptions()
      {
         var options = new[]
         {
            new SelectDistinctFilterOption { Name = "Opt1", Value = "Opt1", IsChecked = true, },
            new SelectDistinctFilterOption { Name = "Opt2", Value = "Opt2", IsChecked = false, },
         };
         var sut = new VMSelectDistinctFilterControl(options);

         sut.ClearFilterCommand.Execute(null);

         Assert.IsFalse(sut.IsAllSelected);
         Assert.IsFalse(sut.HasSomeSelected);
         Assert.IsTrue(options.All(x => x.IsChecked == false));
      }

      [Test]
      public void VMSelectDistinctFilterControl_CheckingSelecAllShouldSelectAllOptions()
      {
         var options = new[]
         {
            new SelectDistinctFilterOption { Name = "Opt1", Value = "Opt1", IsChecked = false, },
            new SelectDistinctFilterOption { Name = "Opt2", Value = "Opt2", IsChecked = false, },
         };
         var sut = new VMSelectDistinctFilterControl(options);

         sut.SelectAllCommand.Execute(true);

         Assert.IsTrue(sut.IsAllSelected);
         Assert.IsTrue(sut.HasSomeSelected);
         Assert.IsTrue(options.All(x => x.IsChecked));
      }

      [Test]
      public void VMSelectDistinctFilterControl_UnCheckingSelecAllShouldUnSelectAllOptions()
      {
         var options = new[]
         {
            new SelectDistinctFilterOption { Name = "Opt1", Value = "Opt1", IsChecked = true, },
            new SelectDistinctFilterOption { Name = "Opt2", Value = "Opt2", IsChecked = true, },
         };
         var sut = new VMSelectDistinctFilterControl(options);

         sut.SelectAllCommand.Execute(false);

         Assert.IsFalse(sut.IsAllSelected);
         Assert.IsFalse(sut.HasSomeSelected);
         Assert.IsTrue(options.All(x => x.IsChecked == false));
      }

      [Test]
      public void VMSelectDistinctFilterControl_UnCheckingAnOptionWhenAllSelectedShouldUnCheckIsAllSelected()
      {
         var options = new[]
         {
            new SelectDistinctFilterOption { Name = "Opt1", Value = "Opt1", IsChecked = true, },
            new SelectDistinctFilterOption { Name = "Opt2", Value = "Opt2", IsChecked = true, },
         };

         var sut = new VMSelectDistinctFilterControl(options);
         options.First().IsChecked = false;
         sut.FilterOptionClickCommand.Execute(null);

         Assert.IsFalse(sut.IsAllSelected);
         Assert.IsTrue(sut.HasSomeSelected);
      }
   }
}
