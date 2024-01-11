// <copyright file="ContextMenuBase.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.ObjectModel;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;

namespace Mitutoyo.MiCAT.ReportModule.GUI.ContextMenu
{
   public abstract class ContextMenuBase : VMBase, IMainContextMenu
   {
      protected int X;
      protected int VisualY;
      protected ObservableCollection<ContextMenuItem> _items;

      public ObservableCollection<ContextMenuItem> Items => _items;
      public void SetPagePosition(int x, int visualY)
      {
         X = x;
         VisualY = visualY;
      }

      protected ContextMenuBase() { }
   }

   public interface IMainContextMenu
   {
      ObservableCollection<ContextMenuItem> Items { get; }
      void SetPagePosition(int x, int visualY);
   }
}
