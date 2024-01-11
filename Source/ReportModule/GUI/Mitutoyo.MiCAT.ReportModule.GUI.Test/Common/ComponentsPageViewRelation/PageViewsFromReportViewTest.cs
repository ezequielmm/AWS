// <copyright file="PageViewsFromReportViewTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.ComponentsPageViewRelation;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Common.ComponentsPageViewRelation
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class PageViewsFromReportViewTest
   {
      [Test]
      [Apartment(ApartmentState.STA)]
      public void ShouldReturnEmpty()
      {
         var sut = new PageViewsFromReportView();
         var reportToTest = new ReportViewToTest();

         reportToTest.Render();

         var result = sut.GetPageViews(reportToTest.ReportView);

         Assert.AreEqual(0, result.Count());
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void ShouldReturnOnePage()
      {
         var sut = new PageViewsFromReportView();
         var reportToTest = new ReportViewToTest();

         reportToTest.AddPage();

         reportToTest.Render();

         var result = sut.GetPageViews(reportToTest.ReportView);

         Assert.AreEqual(1, result.Count());
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void ShouldReturnTwoPages()
      {
         var sut = new PageViewsFromReportView();
         var reportToTest = new ReportViewToTest();

         reportToTest.AddPage();
         reportToTest.AddPage();

         reportToTest.Render();

         var result = sut.GetPageViews(reportToTest.ReportView);

         Assert.AreEqual(2, result.Count());
      }
   }
}
