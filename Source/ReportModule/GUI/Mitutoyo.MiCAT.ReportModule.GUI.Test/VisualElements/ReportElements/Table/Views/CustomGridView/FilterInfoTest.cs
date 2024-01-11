// <copyright file="FilterInfoTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter;
using NUnit.Framework;
using static Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter.FilterInfo;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.Table.Views.CustomGridView
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class FilterInfoTest
   {
      [Test]
      public void FilterInfoTest_TwoInfosWithSameValuesShouldBeEquals()
      {
         var filterInfo1 = new FilterInfo
         {
            IsFilterable = true,
            SelectedValues = new[] { "Value1", "Value2" },
            Values = new[] { new FilterValueItem { DisplayText = "Value1", Value = "Value1" } },
         };

         var filterInfo2 = new FilterInfo
         {
            IsFilterable = true,
            SelectedValues = new[] { "Value1", "Value2" },
            Values = new[] { new FilterValueItem { DisplayText = "Value1", Value = "Value1" } },
         };

         var areEquals = filterInfo1.Equals(filterInfo2);

         Assert.True(areEquals);
      }

      [Test]
      public void FilterInfoTest_TwoInfosWithDifferentIsFiltrableShouldNotBeEquals()
      {
         var filterInfo1 = new FilterInfo
         {
            IsFilterable = true,
            SelectedValues = new[] { "Value1", "Value2" },
            Values = new[] { new FilterValueItem { DisplayText = "Value1", Value = "Value1" } },
         };

         var filterInfo2 = new FilterInfo
         {
            IsFilterable = false,
            SelectedValues = new[] { "Value1", "Value2" },
            Values = new[] { new FilterValueItem { DisplayText = "Value1", Value = "Value1" } },
         };

         var areEquals = filterInfo1.Equals(filterInfo2);

         Assert.False(areEquals);
      }

      [Test]
      public void FilterInfoTest_TwoInfosWithDifferentSelectedValuesShouldNotBeEquals()
      {
         var filterInfo1 = new FilterInfo
         {
            IsFilterable = true,
            SelectedValues = new[] { "Value1", "Value2" },
            Values = new[] { new FilterValueItem { DisplayText = "Value1", Value = "Value1" } },
         };

         var filterInfo2 = new FilterInfo
         {
            IsFilterable = true,
            SelectedValues = new[] { "OtherValue1" },
            Values = new[] { new FilterValueItem { DisplayText = "Value1", Value = "Value1" } },
         };

         var areEquals = filterInfo1.Equals(filterInfo2);

         Assert.False(areEquals);
      }

      [Test]
      public void FilterInfoTest_TwoInfosWithDifferentValuesShouldNotBeEquals()
      {
         var filterInfo1 = new FilterInfo
         {
            IsFilterable = true,
            SelectedValues = new[] { "Value1", "Value2" },
            Values = new[] { new FilterValueItem { DisplayText = "Value1", Value = "Value1" } },
         };

         var filterInfo2 = new FilterInfo
         {
            IsFilterable = true,
            SelectedValues = new[] { "Value1", "Value2" },
            Values = new[] { new FilterValueItem { DisplayText = "Other", Value = "Other" } },
         };

         var areEquals = filterInfo1.Equals(filterInfo2);

         Assert.False(areEquals);
      }
   }
}
