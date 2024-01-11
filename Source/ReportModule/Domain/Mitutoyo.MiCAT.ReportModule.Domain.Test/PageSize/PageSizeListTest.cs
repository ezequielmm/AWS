// <copyright file="PageSizeListTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Test
{
   // Because these tests access a file they are integration tests
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class PageSizeListTest
   {
      [Test]
      public void Initialization_ShouldLoadListFromFile()
      {
         // arrange
         var pageSizeList = new PageSizeList();

         // assert
         Assert.AreEqual(2, pageSizeList.ListCount);
      }

      [Test]
      public void FindPageSize_ShouldFindProperPageSizeInfo()
      {
         // arrange
         var pageSizeList = new PageSizeList();

         // act
         var pageSizeInfo = pageSizeList.FindPageSize(PaperKind.A4);

         // assert
         Assert.AreEqual(PaperKind.A4, pageSizeInfo.PaperKind);
         Assert.AreEqual(1122, pageSizeInfo.Height);
         Assert.AreEqual(794, pageSizeInfo.Width);
      }
   }
}
