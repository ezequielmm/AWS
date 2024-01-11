// <copyright file="ColumnWidthsChangedEventArgsTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView;
using NUnit.Framework;
using static Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.ColumnWidthsChangedEventArgs;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.Table.CustomGridView
{
   public class ColumnWidthsChangedEventArgsTest
   {
      [Test]
      public void ColumnWidthsChangedEventArgs_ShouldInitializeValues()
      {
         var columnWidthInfos = new[]
         {
            new ColumnWidthInfo("ColumnName0", 100),
            new ColumnWidthInfo("ColumnName1", 101),
            new ColumnWidthInfo("ColumnName2", 102),
            new ColumnWidthInfo("ColumnName3", 103),
         };

         var args = new ColumnWidthsChangedEventArgs(columnWidthInfos);

         Assert.IsNotNull(args);
         Assert.IsNotNull(args.ColumnWidthInfos);
         Assert.IsNotEmpty(args.ColumnWidthInfos);
         CollectionAssert.AreEqual(columnWidthInfos, args.ColumnWidthInfos);
      }
   }
}
