// <copyright file="VMZoomFactorTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.Zoom;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.PageLayout
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMZoomFactorTest
   {
      private ZoomInOutCommandArgs _args;

      [SetUp]
      public void SetUp_ZoomCommandArgs_TestData()
      {
         _args = new ZoomInOutCommandArgs();
      }

      [Test]
      public void VMZoomFactor_ShouldGetZoomOptions()
      {
         // Arrange
         var sut = new VMZoomFactor();

         // Act
         var options = sut.ZoomOptions;

         // Assert
         Assert.AreEqual(options.Count, 7);
         Assert.AreEqual(options[0].Value, 2);
         Assert.AreEqual(options[0].Text, "200%");
         Assert.AreEqual(options[1].Value, 1.5);
         Assert.AreEqual(options[1].Text, "150%");
         Assert.AreEqual(options[2].Value, 1.25);
         Assert.AreEqual(options[2].Text, "125%");
         Assert.AreEqual(options[3].Value, 1);
         Assert.AreEqual(options[3].Text, "100%");
         Assert.AreEqual(options[4].Value, 0.75);
         Assert.AreEqual(options[4].Text, "75%");
         Assert.AreEqual(options[5].Value, 0.5);
         Assert.AreEqual(options[5].Text, "50%");
         Assert.AreEqual(options[6].Value, 0.25);
         Assert.AreEqual(options[6].Text, "25%");
      }

      [Test]
      [TestCase(4.0)]
      public void VMZoomFactor_ShouldGetScale(double scale)
      {
         // Arrange
         var sut = new VMZoomFactor();

         // Act
         sut.Scale = scale;

         // Assert
         Assert.AreEqual(scale, sut.Scale);
      }

      [Test]
      [TestCase(1.2)]
      public void VMZoomFactor_ShouldNotifyPropertyChanged(double scale)
      {
         // Arrange
         var sut = new VMZoomFactor { Scale = 1.1 };

         // Act
         var logger = new NotifyPropertyChangedLogger(sut);
         sut.Scale = scale;

         // Assert
         Assert.AreEqual(1, logger.ChangeLog.Count);
         Assert.AreEqual(nameof(sut.Scale), logger.ChangeLog[0]);
      }
      [Test]
      [TestCase(1.1)]
      public void VMZoomFactor_ShouldNotNotifyPropertyChanged(double scale)
      {
         // Arrange
         var sut = new VMZoomFactor { Scale = scale };

         // Act
         var logger = new NotifyPropertyChangedLogger(sut);
         sut.Scale = scale;

         // Assert
         Assert.AreEqual(0, logger.ChangeLog.Count);
      }

      [Test]
      [TestCase(1.1, 1.2)]
      public void VMZoomFactor_ShouldZoomInAndScaleUp(double initialScale, double expectedScale)
      {
         // Arrange
         var sut = new VMZoomFactor { Scale = initialScale };
         _args.ZoomCommand = Operation.ZoomIn;

         // Act
         sut.ZoomCommand.Execute(_args);

         // Assert
         Assert.AreEqual(expectedScale, sut.Scale);
      }

      [TestCase(1.0, 30, 4.0)]
      public void VMZoomFactor_ShouldZoomInAndScaleUpMultiCount(double initialScale, int zoomInCount, double expectedScale)
      {
         // Arrange
         var sut = new VMZoomFactor { Scale = initialScale };
         _args.ZoomCommand = Operation.ZoomIn;

         // Act
         for (int n = 0; n < zoomInCount; n++)
         {
            sut.ZoomCommand.Execute(_args);
         }

         // Assert
         Assert.AreEqual(expectedScale, sut.Scale);
      }
      [Test]
      [TestCase(4.0, 4.0)]
      public void VMZoomFactor_ShouldZoomInAndKeepLimit(double initialScale, double expectedScale)
      {
         // Arrange
         var sut = new VMZoomFactor { Scale = initialScale };
         _args.ZoomCommand = Operation.ZoomIn;

         // Act
         sut.ZoomCommand.Execute(_args);

         // Assert
         Assert.AreEqual(expectedScale, sut.Scale);
      }
      [Test]
      [TestCase(1.1, 1.0)]
      public void VMZoomFactor_ShouldZoomOutAndScaleDown(double initialScale, double expectedScale)
      {
         // Arrange
         var sut = new VMZoomFactor { Scale = initialScale };

         // Act
         _args.ZoomCommand = Operation.ZoomOut;
         sut.ZoomCommand.Execute(_args);

         // Assert
         Assert.AreEqual(expectedScale, sut.Scale);
      }
      [Test]
      [TestCase(0.1, 0.1)]
      public void VMZoomFactor_ShouldZoomOutAndKeepLimit(double initialScale, double expectedScale)
      {
         // Arrange
         var sut = new VMZoomFactor { Scale = initialScale };

         // Act
         _args.ZoomCommand = Operation.ZoomOut;
         sut.ZoomCommand.Execute(_args);

         // Assert
         Assert.AreEqual(expectedScale, sut.Scale);
      }

      [Test]
      [TestCase(0.5, 2)]
      [TestCase(1, 3)]
      [TestCase(2, 4)]
      [Apartment(ApartmentState.STA)]
      public void VMZoomFactor_ShouldChangeSelection(double initialScale, double expectedScale)
      {
         // Arrange
         var sut = new VMZoomFactor { Scale = initialScale };

         // Act
         var zoomLevel = new VMZoomLevel(expectedScale);
         sut.SelectionChangedCommand.Execute(zoomLevel);

         // Assert
         Assert.AreEqual(expectedScale, sut.Scale);
      }
   }
}
