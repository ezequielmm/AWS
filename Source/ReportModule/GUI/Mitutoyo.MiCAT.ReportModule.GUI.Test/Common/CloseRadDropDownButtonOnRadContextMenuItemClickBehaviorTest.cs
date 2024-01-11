// <copyright file="CloseRadDropDownButtonOnRadContextMenuItemClickBehaviorTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using NUnit.Framework;
using Telerik.Windows;
using Telerik.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Common
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class CloseRadDropDownButtonOnRadContextMenuItemClickBehaviorTest
   {
      [Test]
      [Apartment(ApartmentState.STA)]
      public void ShouldCloseRadDropDownButtonWhenRadMenuItemClicked()
      {
         var radDropDownButton = new RadDropDownButton();
         var radMenuItem = new RadMenuItem();
         var radContextMenu = new RadContextMenu();
         radContextMenu.Items.Add(radMenuItem);

         CloseRadDropDownButtonOnRadContextMenuItemClickBehavior
            .SetRadContextMenu(radDropDownButton, radContextMenu);

         radDropDownButton.IsOpen = true;
         radMenuItem.RaiseEvent(new RadRoutedEventArgs(RadMenuItem.ClickEvent));

         Assert.IsFalse(radDropDownButton.IsOpen);
      }
   }
}
