// <copyright file="SelectionAdornerTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows.Media;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Containers.InteractiveControlContainer;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.Containers.InteractiveControlContainer
{
   using InteractiveControlContainer = GUI.VisualElements.Containers.InteractiveControlContainer.InteractiveControlContainer;

   [ExcludeFromCodeCoverage]
   public class SelectionAdornerTests
   {
      [Test]
      [Apartment(ApartmentState.STA)]
      public void Initialize_ShouldAddSelectionMarkers()
      {
         var expectedMarkerCount = 8;

         var container = new InteractiveControlContainer();
         var adorner = new SelectionAdorner(container);

         Assert.IsNotNull(adorner);
         Assert.AreEqual(expectedMarkerCount, VisualTreeHelper.GetChildrenCount(adorner));

         for (var i = 0; i < expectedMarkerCount; i++)
         {
            var marker = VisualTreeHelper.GetChild(adorner, i);

            Assert.IsNotNull(marker);
            Assert.AreEqual(typeof(SelectionMarker), marker.GetType());
         }
      }
   }
}
