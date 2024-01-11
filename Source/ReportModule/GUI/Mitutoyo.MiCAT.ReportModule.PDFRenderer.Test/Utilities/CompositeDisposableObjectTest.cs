// <copyright file="CompositeDisposableObjectTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Test.Utilities
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class CompositeDisposableObjectTest
   {
      [Test]
      public void Add_ShouldAddAComponentIntoTheList()
      {
         // Arrange
         var componentMock = new Mock<IDisposable>();
         var composite = new CompositeDisposableObject();

         // Act
         composite.Add(componentMock.Object);

         // Assert
         Assert.IsTrue(composite.HasComponents());
      }

      [Test]
      public void Dispose_ShouldDisposeComponentAndClearList()
      {
         // Arrange
         var componentMock = new Mock<IDisposable>();
         var composite = new CompositeDisposableObject();
         composite.Add(componentMock.Object);

         // Act
         composite.Dispose();

         // Assert
         Assert.IsFalse(composite.HasComponents());
      }
   }
}
