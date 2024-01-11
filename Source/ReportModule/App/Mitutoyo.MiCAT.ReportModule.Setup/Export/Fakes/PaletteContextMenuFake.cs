// <copyright file="PaletteContextMenuFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI.ContextMenu;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Export.Fakes
{
   [ExcludeFromCodeCoverage]
   public class PaletteContextMenuFake : IMainContextMenu
   {
      public ObservableCollection<ContextMenuItem> Items => new ObservableCollection<ContextMenuItem>();

      public void SetPagePosition(int x, int visualY)
      {
      }
   }
}
