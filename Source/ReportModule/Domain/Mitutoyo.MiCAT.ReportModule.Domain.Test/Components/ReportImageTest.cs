// <copyright file="ReportImageTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Test.Components
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportImageTest
   {
      [Test]
      public void With_ShouldHaveANewImage()
      {
         // Arrange
         var placement = new ReportComponentPlacement(new Id<ReportBody>(Guid.NewGuid()), 10, 10, 10, 10);
         var reportImage = new ReportImage(placement, "Old Image");

         // Act
         var newReportImage = reportImage.WithImage("newImageText");

         // Assert
         Assert.AreEqual("newImageText", newReportImage.Image);
         Assert.AreEqual(placement, newReportImage.Placement);
      }
   }
}
