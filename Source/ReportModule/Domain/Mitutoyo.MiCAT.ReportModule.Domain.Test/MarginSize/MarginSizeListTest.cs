// <copyright file="MarginSizeListTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Test.MarginSize
{
   // Because these tests access a file they are integration tests
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class MarginSizeListTest
   {
      [Test]
      public void InitializationShouldLoadFromFile()
      {
         // arrange
         var marginSizeList = new MarginSizeList();

         // assert
         Assert.AreEqual(2, marginSizeList.ListCount);
         Assert.IsNotEmpty(marginSizeList.MarginSizeInfoList);
      }
      [Test]
      public void FindMarginSize_ShouldFindProperMarginSizeInfo()
      {
         // arrange
         var marginSizeList = new MarginSizeList();

         // act
         var marginSizeInfo = marginSizeList.FindMarginSize(MarginKind.Normal);

         // assert
         Assert.AreEqual(MarginKind.Normal, marginSizeInfo.MarginKind);
         Assert.AreEqual(96, marginSizeInfo.Top);
         Assert.AreEqual(0, marginSizeInfo.Right);
         Assert.AreEqual(96, marginSizeInfo.Bottom);
         Assert.AreEqual(0, marginSizeInfo.Left);
      }
   }
}
