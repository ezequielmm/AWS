// <copyright file="AppStateModuleTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Mitutoyo.MiCAT.ApplicationState;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Test
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class AppStateModuleTest
   {
      [Test]
      public void Test_ReadRecentSnapShot()
      {
         // Arrange
         var snapShotId = new SnapShotId(Guid.NewGuid());
         var history = new Mock<IAppStateHistory>();
         var initialSnapShot = new Mock<ISnapShot>();
         var replacedSnapShot = new Mock<ISnapShot>();
         var readSnapShot = new Mock<ISnapShot>();

         history.Setup(h => h.ReadSnapShotId(null)).Returns(snapShotId);
         history.Setup(h => h.ReadSnapShot(It.IsAny<SnapShotId>(), It.IsAny<ISnapShot>(), Variant.Full)).Returns(readSnapShot.Object);
         initialSnapShot.Setup(h => h.ReplaceWith(readSnapShot.Object, AppStateKinds.NonUndoable)).Returns(replacedSnapShot.Object);

         MethodInfo methodToTest = typeof(AppStateModule).GetMethod("ReadRecentSnapShot", BindingFlags.NonPublic | BindingFlags.Static);
         object[] parameters = { history.Object, initialSnapShot.Object };

         // Act
         var resultSnapShot = methodToTest.Invoke(null, parameters);

         // Assert
         history.Verify(m => m.ReadSnapShot(snapShotId, initialSnapShot.Object, Variant.Full), Times.Once);
         initialSnapShot.Verify(m => m.ReplaceWith(readSnapShot.Object, AppStateKinds.NonUndoable), Times.Once);
         Assert.AreEqual(replacedSnapShot.Object, resultSnapShot);
      }

      [Test]
      public void Test_ReadRecentSnapShot_SerializationException()
      {
         // Arrange
         var snapShotId = new SnapShotId(Guid.NewGuid());
         var history = new Mock<IAppStateHistory>();
         var replacedSnapShot = new Mock<ISnapShot>();
         var initialSnapShot = new Mock<ISnapShot>();
         var readSnapShot = new Mock<ISnapShot>();

         history.Setup(h => h.ReadSnapShotId(null)).Returns(snapShotId);
         history.Setup(h => h.ReadSnapShot(It.IsAny<SnapShotId>(), It.IsAny<ISnapShot>(), Variant.Full)).Throws(new StateSerializationException("Test Exception Message"));
         initialSnapShot.Setup(h => h.ReplaceWith(readSnapShot.Object, AppStateKinds.NonUndoable)).Returns(replacedSnapShot.Object);

         MethodInfo methodToTest = typeof(AppStateModule).GetMethod("ReadRecentSnapShot", BindingFlags.NonPublic | BindingFlags.Static);
         object[] parameters = { history.Object, initialSnapShot.Object };

         // Act
         var resultSnapShot = methodToTest.Invoke(null, parameters);

         // Assert
         history.Verify(m => m.ReadSnapShot(snapShotId, initialSnapShot.Object, Variant.Full), Times.Once);
         initialSnapShot.Verify(m => m.ReplaceWith(readSnapShot.Object, AppStateKinds.NonUndoable), Times.Never);
         Assert.AreEqual(initialSnapShot.Object, resultSnapShot);
      }
   }
}
