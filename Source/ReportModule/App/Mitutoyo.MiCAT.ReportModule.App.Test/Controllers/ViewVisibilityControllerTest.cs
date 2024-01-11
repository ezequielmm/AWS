// <copyright file="ViewVisibilityControllerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Visibility;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.App.Test.Controllers
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ViewVisibilityControllerTest
   {
      [Test]
      public void ToggleVisibilityShouldCallToRun()
      {
         //Arrange
         var currentSnapShot = new Mock<ISnapShot>();
         currentSnapShot.Setup(s => s.GetItems<ViewVisibility>()).Returns(new[] { new ViewVisibility(ViewElement.Runs, false) });

         var appStateHistoryMock = new Mock<IAppStateHistory>();
         appStateHistoryMock.SetupGet(h => h.CurrentSnapShot).Returns(currentSnapShot.Object);

         var sut = new ViewVisibilityController(appStateHistoryMock.Object);

         //Act
         sut.ToggleVisibility(ViewElement.Runs);

         //Assert
         appStateHistoryMock.Verify(h => h.Run(It.IsAny<Expression<Func<ISnapShot, ISnapShot>>>()), Times.Once);
      }

      [Test]
      public void UpdateVisibilityShouldCallToRun()
      {
         //Arrange
         var currentSnapShot = new Mock<ISnapShot>();
         currentSnapShot.Setup(s => s.GetItems<ViewVisibility>()).Returns(new[] { new ViewVisibility(ViewElement.Runs, false) });

         var appStateHistoryMock = new Mock<IAppStateHistory>();
         appStateHistoryMock.SetupGet(h => h.CurrentSnapShot).Returns(currentSnapShot.Object);

         var sut = new ViewVisibilityController(appStateHistoryMock.Object);

         //Act
         sut.UpdateVisibility(ViewElement.Runs, true);

         //Assert
         appStateHistoryMock.Verify(h => h.Run(It.IsAny<Expression<Func<ISnapShot, ISnapShot>>>()), Times.Once);
      }
   }
}
