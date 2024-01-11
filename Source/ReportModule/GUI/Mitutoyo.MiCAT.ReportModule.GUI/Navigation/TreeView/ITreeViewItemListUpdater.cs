// <copyright file="ITreeViewItemListUpdater.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Navigation.TreeView
{
   public interface ITreeViewItemListUpdater
   {
      void UpdateTreeViewItems(IList<IVMNodeTreeViewItem> existingItems, IEnumerable<IVMNodeTreeViewItem> newItems);
   }
}
