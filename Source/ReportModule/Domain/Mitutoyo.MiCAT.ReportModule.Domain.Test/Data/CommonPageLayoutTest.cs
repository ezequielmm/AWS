// <copyright file="CommonPageLayoutTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Test.Data
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class CommonPageLayoutTest
   {
      [Test]
      public void With_AddANewPageSizeInfo()
      {
         PageSizeInfo pageSizeInfo = new PageSizeInfo(PaperKind.A2, 1, 1);

         // Arrange
         var commonPageLayout = new CommonPageLayout();

         // Act
         commonPageLayout = commonPageLayout.With(pageSizeInfo);

         // Assert
         Assert.IsNotNull(commonPageLayout.PageSize);
         Assert.AreEqual(commonPageLayout.PageSize.PaperKind, PaperKind.A2);
      }
   }
}
