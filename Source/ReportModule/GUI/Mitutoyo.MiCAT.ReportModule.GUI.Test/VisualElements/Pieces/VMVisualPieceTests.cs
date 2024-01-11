// <copyright file="VMVisualPieceTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.Pieces
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class VMVisualPieceTests
   {
      [Test]
      public void Constructor_ShouldSetOwner()
      {
         var component = Mock.Of<IVMReportComponent>();
         var vmPlacement = Mock.Of<IVMVisualPlacement>();
         var element = new VMVisualElementPiece(vmPlacement, component);

         Assert.IsNotNull(element);
         Assert.IsNotNull(element.Owner);
         Assert.AreEqual(component, element.Owner);
      }
   }
}
