// <copyright file="CustomFilterEventArgsTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter;
using NUnit.Framework;
using static Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter.VMSelectDistinctFilterControl;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.Table.Views.CustomGridView
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class CustomFilterEventArgsTest
   {
      [SetUp]
      public void Setup()
      {
      }

      [Test]
      public void CustomFilterEventArgs_ShouldInitializeSelectedItems()
      {
         var selectedItems = new[]
         {
            new SelectDistinctFilterOption { Name = "Opt1", Value = "Opt1", },
            new SelectDistinctFilterOption { Name = "Opt2", Value = "Opt2", },
         };

         var args = new CustomFilterEventArgs(selectedItems);

         Assert.IsNotNull(args.SelectedItems);
         CollectionAssert.AreEqual(selectedItems, args.SelectedItems);
      }
   }
}
