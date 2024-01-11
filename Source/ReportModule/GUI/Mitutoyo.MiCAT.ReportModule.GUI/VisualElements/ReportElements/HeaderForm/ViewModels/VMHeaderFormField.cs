// <copyright file="VMHeaderFormField.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Clients;
using Mitutoyo.MiCAT.Core;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels.ValueResolvers;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels
{
   public class VMHeaderFormField : VMBase, IUpdateClient, IDisposable
   {
      private readonly IAppStateHistory _appStateHistory;
      private readonly IHeaderFormFieldController _headerFormFieldController;
      private Guid _selectedFieldId;
      private string _fieldLabel;
      private string _selectedFieldValue;
      private bool _canEdit;
      private bool _isEditable;
      private double _labelWidthPercentage;

      private ICommand _selectedFieldChangedCommand;
      private ICommand _updateLabelCommand;
      private ICommand _editLabelCommand;
      private ICommand _clearLabelCommand;
      private ICommand _updateLabelWidthPercentageCommand;

      public VMHeaderFormField(
         IAppStateHistory appStateHistory,
         IHeaderFormFieldController headerFormFieldController,
         IItemId<ReportHeaderFormField> fieldId,
         int fieldIndex,
         VMHeaderFormRow headerFormRow)
      {
         _headerFormFieldController = headerFormFieldController;

         FieldId = fieldId;
         FieldIndex = fieldIndex;
         Row = headerFormRow;

         _appStateHistory = appStateHistory.AddClient(this);
         SubscribeToAppStateUpdates();
         InitializeCommands();

         Update(_appStateHistory.CurrentSnapShot);
      }

      public int FieldIndex { get; private set; }
      public IVMHeaderForm HeaderForm => Row.HeaderForm;
      public int RowIndex => Row.RowIndex;
      public VMHeaderFormRow Row { get; private set; }

      public Name Name => this.GetType().GetTypeName();

      public IItemId<ReportHeaderFormField> FieldId { get; private set; }

      public Guid SelectedFieldId
      {
         get => _selectedFieldId;
         set
         {
            _selectedFieldId = value;
            RaisePropertyChanged();
         }
      }

      public string FieldLabel
      {
         get => _fieldLabel;
         set
         {
            _fieldLabel = value;
            RaisePropertyChanged();
         }
      }

      public string SelectedFieldValue
      {
         get => _selectedFieldValue;
         set
         {
            _selectedFieldValue = value;
            RaisePropertyChanged();
         }
      }

      public bool IsEditable
      {
         get => _isEditable;
         set
         {
            _isEditable = value;
            RaisePropertyChanged();
         }
      }

      public bool CanEdit
      {
         get => _canEdit;
         set
         {
            _canEdit = value;
            RaisePropertyChanged();
         }
      }

      public double LabelWidthPercentage
      {
         get => _labelWidthPercentage;
         set
         {
            _labelWidthPercentage = value;
            RaisePropertyChanged();
         }
      }

      public ICommand SelectedFieldChangedCommand
      {
         get => _selectedFieldChangedCommand;
         private set
         {
            if (_selectedFieldChangedCommand != value)
            {
               _selectedFieldChangedCommand = value;
               RaisePropertyChanged();
            }
         }
      }

      public ICommand UpdateLabelCommand
      {
         get => _updateLabelCommand;
         private set
         {
            if (_updateLabelCommand != value)
            {
               _updateLabelCommand = value;
               RaisePropertyChanged();
            }
         }
      }

      public ICommand EditLabelCommand
      {
         get => _editLabelCommand;
         private set
         {
            if (_editLabelCommand != value)
            {
               _editLabelCommand = value;
               RaisePropertyChanged();
            }
         }
      }

      public ICommand ClearLabelCommand
      {
         get => _clearLabelCommand;
         private set
         {
            if (_clearLabelCommand != value)
            {
               _clearLabelCommand = value;
               RaisePropertyChanged();
            }
         }
      }

      public ICommand UpdateLabelWidthPercentageCommand
      {
         get => _updateLabelWidthPercentageCommand;
         private set
         {
            if (_updateLabelWidthPercentageCommand != value)
            {
               _updateLabelWidthPercentageCommand = value;
               RaisePropertyChanged();
            }
         }
      }

      public Task Update(ISnapShot snapShot)
      {
         OnUpdate(snapShot);

         return Task.CompletedTask;
      }

      public virtual void Dispose()
      {
         SelectedFieldChangedCommand = null;
         UpdateLabelCommand = null;
         EditLabelCommand = null;
         ClearLabelCommand = null;
         UpdateLabelWidthPercentageCommand = null;

         _appStateHistory?.RemoveClient(this);
      }

      private void SubscribeToAppStateUpdates()
      {
         var appStateSubscription = new Subscription(this)
            .With(new IdFilter(FieldId))
            .With(new TypeFilter(typeof(RunSelection)));
         _appStateHistory.Subscribe(appStateSubscription);
      }

      private void InitializeCommands()
      {
         SelectedFieldChangedCommand = new RelayCommand<object>(OnSelectedFieldChangedCommand);
         UpdateLabelCommand = new RelayCommand<string>(OnUpdateLabelCommand);
         EditLabelCommand = new RelayCommand<object>(OnEditLabelCommand);
         ClearLabelCommand = new RelayCommand<object>(OnClearLabelCommand);
         UpdateLabelWidthPercentageCommand = new RelayCommand<object>(OnUpdateLabelWidthPercentageCommand);
      }

      private void OnUpdate(ISnapShot snapShot)
      {
         if (!snapShot.HasBeenDeleted(FieldId))
         UpdateFromSnapShot(snapShot);

         UpdateFieldValueFromSnapShot(snapShot);
      }

      private void UpdateFromSnapShot(ISnapShot snapShot)
      {
         var headerFormField = snapShot.GetItem<ReportHeaderFormField>(FieldId);

         SelectedFieldId = headerFormField.SelectedFieldId;
         CanEdit = SelectedFieldId != VMDynamicPropertyItem.EmptyDynamicPropertyItem.Id;
         IsEditable = headerFormField.CustomLabel != null;
         FieldLabel = headerFormField.CustomLabel ?? SelectedDynamicPropertyItem.DisplayName;
         LabelWidthPercentage = headerFormField.LabelWidthPercentage;
      }

      private void UpdateFieldValueFromSnapShot(ISnapShot snapShot)
      {
         var valueResolver = new DynamicPropertyValueResolver(() => snapShot.GetCurrentDynamicPropertyValues(), SelectedDynamicPropertyItem);
         var value = valueResolver.GetValue();
         SelectedFieldValue = value;
      }

      private void OnSelectedFieldChangedCommand(object obj = null)
      {
         if (ShouldUpdateSelectedHeaderFormField())
            _headerFormFieldController.UpdateSelectedHeaderFormField(FieldId, SelectedFieldId);
      }

      private bool ShouldUpdateSelectedHeaderFormField()
      {
         return _appStateHistory.CurrentSnapShot.GetItem(FieldId).SelectedFieldId != SelectedFieldId;
      }

      private void OnUpdateLabelCommand(string value)
      {
         if (CanEdit && IsEditable)
         {
            if (IsCustomFieldLabel(value))
            {
               if (FieldLabelHasChanged(value))
               {
                  UpdateFieldLabel(value);
               }
            }
            else
            {
               IsEditable = false;
            }
         }
      }

      private void OnEditLabelCommand(object args)
      {
         IsEditable = true;
      }

      private void OnClearLabelCommand(object obj = null)
      {
         IsEditable = false;
         FieldLabel = SelectedDynamicPropertyItem.DisplayName;
         _headerFormFieldController.UpdateSelectedHeaderFormFieldLabel(FieldId, null);
      }

      private void OnUpdateLabelWidthPercentageCommand(object args)
      {
         _headerFormFieldController.UpdateSelectedHeaderFormFieldLabelWidthPercentage(FieldId, LabelWidthPercentage);
      }

      private bool IsCustomFieldLabel(string value) => value != SelectedDynamicPropertyItem.DisplayName;

      private bool FieldLabelHasChanged(string value) => value != FieldLabel;

      private void UpdateFieldLabel(string value)
      {
         _headerFormFieldController.UpdateSelectedHeaderFormFieldLabel(FieldId, value);
      }

      private VMDynamicPropertyItem SelectedDynamicPropertyItem =>
         HeaderForm.DynamicPropertyItems.First(x => x.Id == SelectedFieldId);
   }
}
