// <copyright file="ReportHeaderFormTest.cs" company="Mitutoyo Europe GmbH">
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
   public class ReportHeaderFormTest
   {
      [Test]
      public void WithPosition_ShouldDataItemIdsBeTheSameAsRowIds()
      {
         //Arrange
         var _headerFormRowIds = new[]
         {
            new Id<ReportHeaderFormRow>(Guid.NewGuid()),
            new Id<ReportHeaderFormRow>(Guid.NewGuid()),
         };

         //Act
         var placement = new ReportComponentPlacement(new Id<ReportBody>(Guid.NewGuid()), 10, 10, 10, 10);
         var component = new ReportHeaderForm(placement, _headerFormRowIds);

         Assert.AreEqual(2, component.RowIds.Count);
         Assert.AreEqual(placement, component.Placement);
         CollectionAssert.AreEquivalent(component.RowIds, component.ReportComponentDataItemIds);
      }
   }
}
