// <copyright file="ContextMenuItem.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Mitutoyo.MiCAT.ReportModule.GUI.ContextMenu
{
   public class ContextMenuItem
   {
      public ContextMenuItem()
      {
         this.SubItems = new ObservableCollection<ContextMenuItem>();
      }

      public string Text { get; set; }
      public ContextMenuItemIcon IconName { get; set; }
      public bool IsSeparator { get; set; }
      public ICommand Command { get; set; }
      public ObservableCollection<ContextMenuItem> SubItems { get; set; }

      public string HotKeyDisplay { get; set; }
   }
}
