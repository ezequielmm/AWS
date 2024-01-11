// <copyright file="IVMNodeTreeViewItem.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Navigation.TreeView
{
   public interface IVMNodeTreeViewItem
   {
      Guid Id { get; set; }
      string Name { get; set; }
      ObservableCollection<IVMNodeTreeViewItem> ChildrenVms { get; }
      IAsyncCommand DeleteItemCommand { get; }
      ICommand SelectItemCommand { get; }
      bool CanDeleteItem { get; set; }
      bool IsSelected { get; set; }
      bool IsExpanded { get; set; }
      bool IsLoading { get; set; }
   }
}