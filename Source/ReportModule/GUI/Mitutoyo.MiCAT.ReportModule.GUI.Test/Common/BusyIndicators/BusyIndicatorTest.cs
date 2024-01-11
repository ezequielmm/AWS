// <copyright file="BusyIndicatorTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.BusyIndicators;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.DispatcherWrapping;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Common.BusyIndicators
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class BusyIndicatorTest
   {
      private Mock<IDispatcherWrapper> _dispatcherWrapperMock;

      [SetUp]
      public void SetUp()
      {
         _dispatcherWrapperMock = new Mock<IDispatcherWrapper>();
      }

      [Test]
      public void BusyIndicatorInitialize()
      {
         // Arrange:
         BusyIndicator busyIndicator;

         // Act:
         busyIndicator = new BusyIndicator(_dispatcherWrapperMock.Object);

         // Assert:
         Assert.IsFalse(busyIndicator.IsBusy);
      }

      [Test]
      public void ShouldSetBusyTrueAndWaitForRenderTest()
      {
         // Arrange:
         var busyIndicator = new BusyIndicator(_dispatcherWrapperMock.Object);

         // Act:
         busyIndicator.SetIsBusyTrueAndWaitForWPFRender();

         // Assert:
         Assert.IsTrue(busyIndicator.IsBusy);
         _dispatcherWrapperMock.Verify(d => d.WaitForWPFToRender(), Times.Once);
      }

      [Test]
      public void ShouldNotWaitForRenderTwiceTest()
      {
         // Arrange:
         var busyIndicator = new BusyIndicator(_dispatcherWrapperMock.Object);

         // Act:
         busyIndicator.SetIsBusyTrueAndWaitForWPFRender();
         busyIndicator.SetIsBusyTrueAndWaitForWPFRender();

         // Assert:
         Assert.IsTrue(busyIndicator.IsBusy);
         _dispatcherWrapperMock.Verify(d => d.WaitForWPFToRender(), Times.Once);
      }

      [Test]
      public void SetIsBusyFalseTest()
      {
         // Arrange:
         var busyIndicator = new BusyIndicator(_dispatcherWrapperMock.Object);
         busyIndicator.SetIsBusyTrueAndWaitForWPFRender();

         // Act:
         busyIndicator.SetIsBusyFalse();

         // Assert:
         Assert.IsFalse(busyIndicator.IsBusy);
      }

      [Test]
      public void SetBusyIndicatorForALongUIOperationTest()
      {
         // Arrange:
         var busyIndicator = new BusyIndicator(_dispatcherWrapperMock.Object);

         // Act:
         busyIndicator.SetIsBusyTrueUntilUIIsIdle();

         // Assert:
         Assert.IsTrue(busyIndicator.IsBusy);
         _dispatcherWrapperMock.Verify(d => d.WaitForWPFToRender(), Times.Once);
         _dispatcherWrapperMock.Verify(d => d.BeginInvokeAfterWPFRenderAsync(busyIndicator.SetIsBusyFalse), Times.Once);
      }

      [Test]
      public void ShouldNotWaitForRenderTwiceWhenSettingBusyIndicatorForALongUIOperationTest()
      {
         // Arrange:
         var busyIndicator = new BusyIndicator(_dispatcherWrapperMock.Object);

         // Act:
         busyIndicator.SetIsBusyTrueUntilUIIsIdle();
         busyIndicator.SetIsBusyTrueUntilUIIsIdle();

         // Assert:
         Assert.IsTrue(busyIndicator.IsBusy);
         _dispatcherWrapperMock.Verify(d => d.WaitForWPFToRender(), Times.Once);
         _dispatcherWrapperMock.Verify(d => d.BeginInvokeAfterWPFRenderAsync(busyIndicator.SetIsBusyFalse), Times.Once);
      }
   }
}
