// <copyright file="InteractiveReportGridViewTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView;
using NUnit.Framework;
using Telerik.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.Table.Views.CustomGridView
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class InteractiveReportGridViewTest
   {
      [Test]
      [Apartment(ApartmentState.STA)]
      public void FInteractiveReportGridView_ShouldBeIntializedProperly()
      {
         var sut = new InteractiveReportGridView();

         Assert.AreEqual("Office2016", StyleManager.GetTheme(sut).ToString());
         Assert.IsNotNull(sut.Header);
      }
   }
}
