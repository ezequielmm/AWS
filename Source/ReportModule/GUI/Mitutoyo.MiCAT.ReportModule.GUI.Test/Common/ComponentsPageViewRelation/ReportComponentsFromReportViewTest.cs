// <copyright file="ReportComponentsFromReportViewTest.cs" company="Mitutoyo Europe GmbH">
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
   public class ReportComponentsFromReportViewTest
   {
      [Test]
      [Apartment(ApartmentState.STA)]
      public void ShouldReturnEmpty()
      {
         // Arrange
         var sut = new ReportComponentsFromReportView();
         var reportToTest = new ReportViewToTest();

         reportToTest.Render();

         // Act
         var result = sut.GetReportComponents(reportToTest.ReportView);

         // Assert
         Assert.AreEqual(0, result.Count());
      }
      [Test]
      [Apartment(ApartmentState.STA)]
      public void ShouldReturnOneComponent()
      {
         // Arrange
         var sut = new ReportComponentsFromReportView();
         var reportToTest = new ReportViewToTest();

         reportToTest.AddElement(0, 10, 20, 20);
         reportToTest.Render();

         // Act
         var result = sut.GetReportComponents(reportToTest.ReportView);

         // Assert
         Assert.AreEqual(1, result.Count());
      }
      [Test]
      [Apartment(ApartmentState.STA)]
      public void ShouldReturnOneComponentOnePiece()
      {
         // Arrange
         var sut = new ReportComponentsFromReportView();
         var reportToTest = new ReportViewToTest();

         reportToTest.AddElement(0, 10, 20, 20);
         reportToTest.AddPiece(50, 10, 20, 20);
         reportToTest.Render();

         // Act
         var result = sut.GetReportComponents(reportToTest.ReportView);

         // Assert
         Assert.AreEqual(2, result.Count());
      }
      [Test]
      [Apartment(ApartmentState.STA)]
      public void ShouldReturnTwoComponentTwoPiece()
      {
         // Arrange
         var sut = new ReportComponentsFromReportView();
         var reportToTest = new ReportViewToTest();

         reportToTest.AddElement(0, 10, 20, 20);
         reportToTest.AddElement(100, 10, 20, 20);
         reportToTest.AddPiece(50, 10, 20, 20);
         reportToTest.AddPiece(150, 10, 20, 20);
         reportToTest.Render();

         // Act
         var result = sut.GetReportComponents(reportToTest.ReportView);

         // Assert
         Assert.AreEqual(4, result.Count());
      }
   }
}
