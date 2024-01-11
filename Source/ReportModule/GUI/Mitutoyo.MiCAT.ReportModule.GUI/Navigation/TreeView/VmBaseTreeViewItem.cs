// <copyright file="VmBaseTreeViewItem.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Navigation.TreeView
{
   public abstract class VmBaseTreeViewItem : VMBase, IVMNodeTreeViewItem, IEquatable<IVMNodeTreeViewItem>
   {
      private bool _isExpanded;
      private bool _isSelected;
      private bool _canDeleteItem;
      private bool _isLoading;

      protected VmBaseTreeViewItem()
      {
         SelectItemCommand = new RelayCommand<object>(_ => OnSelectItem());
         DeleteItemCommand = new AsyncCommand<object>(_ => OnDeleteItem());
         ChildrenVms = new ObservableCollection<IVMNodeTreeViewItem>();
      }

      public Guid Id { get; set; }
      public string Name { get; set; }
      public ObservableCollection<IVMNodeTreeViewItem> ChildrenVms { get; }

      public ICommand SelectItemCommand { get; }

      public IAsyncCommand DeleteItemCommand { get; }

      public bool CanDeleteItem
      {
         get => _canDeleteItem;
         set
         {
            if (_canDeleteItem == value) return;

            _canDeleteItem = value;
            RaisePropertyChanged();
         }
      }

      public bool IsExpanded
      {
         get => _isExpanded;
         set
         {
            if (_isExpanded == value) return;

            _isExpanded = value;
            RaisePropertyChanged();
         }
      }

      public bool IsSelected
      {
         get => _isSelected;
         set
         {
            if (_isSelected == value) return;

            _isSelected = value;
            RaisePropertyChanged();
         }
      }

      public bool IsLoading
      {
         get => _isLoading;
         set
         {
            if (_isLoading == value) return;

            _isLoading = value;
            RaisePropertyChanged();
         }
      }

      protected abstract void OnSelectItem();

      protected virtual Task OnDeleteItem() => Task.CompletedTask;

      public override bool Equals(object obj)
      {
         return Equals(obj as IVMNodeTreeViewItem);
      }

      public bool Equals(IVMNodeTreeViewItem other)
      {
         return !(other is null) && other.Id == Id;
      }
      public override int GetHashCode()
      {
         return Id.GetHashCode();
      }
   }
}
