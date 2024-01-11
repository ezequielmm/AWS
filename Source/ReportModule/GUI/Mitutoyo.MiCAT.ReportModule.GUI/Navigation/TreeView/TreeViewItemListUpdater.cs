// <copyright file="TreeViewItemListUpdater.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Mitutoyo.MiCAT.Core.StdLib.Extensions;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Navigation.TreeView
{
   public class TreeViewItemListUpdater : ITreeViewItemListUpdater
   {
      public void UpdateTreeViewItems(IList<IVMNodeTreeViewItem> existingItems, IEnumerable<IVMNodeTreeViewItem> newItems)
      {
         var itemsToKeep = existingItems.Intersect(newItems);
         var itemsToDelete = existingItems.Except(itemsToKeep);
         var itemsToAdd = newItems.Except(itemsToKeep);

         itemsToDelete
            .ToList()
            .ForEach(x => existingItems.Remove(x));

         itemsToAdd
            .ForEach(x => existingItems.Insert(newItems.IndexOf(x), x));

         var index = 0;

         existingItems
            .ForEach(x => UpdateTreeViewItems(x.ChildrenVms, newItems.ElementAt(index++).ChildrenVms));
      }
   }
}
