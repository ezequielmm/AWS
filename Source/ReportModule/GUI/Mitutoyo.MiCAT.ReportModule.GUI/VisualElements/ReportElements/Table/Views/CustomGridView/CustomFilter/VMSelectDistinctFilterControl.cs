// <copyright file="VMSelectDistinctFilterControl.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Mitutoyo.MiCAT.Common.GUI.Commands;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter
{
   public class VMSelectDistinctFilterControl : VMBase
   {
      private bool _isEmpty;
      private string _filterText = string.Empty;
      private bool _isAllSelected = false;
      private bool _hasSomeSelected = false;
      private CollectionViewSource _distinctOptions;

      public ICommand SearchFilterOptionCommand { get; private set; }
      public ICommand SelectAllCommand { get; private set; }
      public ICommand ApplyFilterCommand { get; private set; }
      public ICommand ClearFilterCommand { get; private set; }
      public ICommand CancelFilterCommand { get; private set; }
      public ICommand FilterOptionClickCommand { get; private set; }

      public event EventHandler<CustomFilterEventArgs> ApplyFilter;
      public event EventHandler<CustomFilterEventArgs> CancelFilter;

      public VMSelectDistinctFilterControl(IEnumerable<SelectDistinctFilterOption> options)
      {
         DistinctOptions = new CollectionViewSource();
         _distinctOptions.Source = options;
         _distinctOptions.Filter += OnDistinctOptions_Filter;
         SearchFilterOptionCommand = new RelayCommand<string>(OnSearchFilterChanged);
         ApplyFilterCommand = new RelayCommand(OnApplyFilterClick);
         SelectAllCommand = new RelayCommand<bool>(OnSelectAllCommand);
         ClearFilterCommand = new RelayCommand(OnClearFilterCommand);
         CancelFilterCommand = new RelayCommand(OnCancelFilterCommand);
         FilterOptionClickCommand = new RelayCommand(OnFilterOptionClick);
         IsEmpty = (options?.Count() ?? 0) == 0;
         OnSelectionUpdated();
      }

      private IEnumerable<SelectDistinctFilterOption> Options =>
         _distinctOptions.View.SourceCollection.Cast<SelectDistinctFilterOption>();

      private void OnSelectionUpdated()
      {
         IsAllSelected = Options.Count() > 0 && Options.All(x => x.IsChecked);
         HasSomeSelected = Options.Any(x => x.IsChecked);
      }

      private void OnFilterOptionClick(object obj)
      {
         OnSelectionUpdated();
      }

      private void OnCancelFilterCommand(object obj)
      {
         CancelFilter?.Invoke(this, null);
      }

      private void OnClearFilterCommand(object obj)
      {
         Options.ToList().ForEach(x => x.IsChecked = false);
         OnSelectionUpdated();
      }

      private void OnSelectAllCommand(bool isChecked)
      {
         Options.ToList().ForEach(x => x.IsChecked = isChecked);
         OnSelectionUpdated();
      }

      private void OnApplyFilterClick(object obj)
      {
         var source = _distinctOptions.Source as IEnumerable<SelectDistinctFilterOption>;
         var selectedItems = source.Where(x => x.IsChecked);

         ApplyFilter?.Invoke(this, new CustomFilterEventArgs(selectedItems));
      }

      private void OnDistinctOptions_Filter(object sender, FilterEventArgs e)
      {
         if (e.Item is SelectDistinctFilterOption option)
         {
            e.Accepted = option != null && option.Name.ToLower().Contains(FilterText.ToLower());
         }
      }

      private void OnSearchFilterChanged(string searchText)
      {
         FilterText = searchText;
      }

      public CollectionViewSource DistinctOptions
      {
         get { return _distinctOptions; }
         set
         {
            _distinctOptions = value;
            RaisePropertyChanged();
         }
      }

      public string FilterText
      {
         get
         {
            return _filterText;
         }
         set
         {
            _filterText = value;
            RaisePropertyChanged();
            DistinctOptions.View.Refresh();
         }
      }

      public bool IsAllSelected
      {
         get
         {
            return _isAllSelected;
         }
         set
         {
            _isAllSelected = value;
            RaisePropertyChanged();
         }
      }

      public bool HasSomeSelected
      {
         get
         {
            return _hasSomeSelected;
         }
         set
         {
            _hasSomeSelected = value;
            RaisePropertyChanged();
         }
      }

      public bool IsEmpty
      {
         get
         {
            return _isEmpty;
         }
         set
         {
            _isEmpty = value;
            RaisePropertyChanged();
         }
      }

      public class SelectDistinctFilterOption : VMBase
      {
         public string Name { get; set; }

         private bool _isChecked;

         public bool IsChecked
         {
            get
            {
               return _isChecked;
            }
            set
            {
               _isChecked = value;
               RaisePropertyChanged();
            }
         }

         public string Value { get; set; }
         public bool IsEnabled { get; set; }
      }
   }
}
