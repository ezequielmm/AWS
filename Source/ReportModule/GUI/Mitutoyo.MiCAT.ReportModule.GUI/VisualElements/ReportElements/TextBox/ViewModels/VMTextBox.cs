// <copyright file="VMTextBox.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows.Input;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Telerik.Windows.Documents.FormatProviders.Xaml;
using Telerik.Windows.Documents.Model;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.TextBox.ViewModels
{
   public class VMTextBox : VMReportComponent
   {
      private string _inputText;
      private string _previousInputText;
      private bool _isEditing;
      private bool _isEmpty;
      private bool _showWatermark;
      private ICommand _startEditCommand;
      private ICommand _endEditCommand;

      private readonly ITextBoxController _textBoxController;

      public VMTextBox(
          IAppStateHistory appStateHistory,
          Id<ReportTextBox> textBoxId,
          IVMReportComponentPlacement vmPlacement,
          IDeleteComponentController deleteComponentController,
          ITextBoxController textBoxController)
          : base(
               appStateHistory,
               textBoxId,
               vmPlacement,
               deleteComponentController)
      {
         StartEditCommand = new RelayCommand<object>(OnStartEdit);
         EndEditCommand = new RelayCommand<object>(OnEndEdit);

         _textBoxController = textBoxController;

         VMPlacement.MinWidth = 50;
         VMPlacement.MinHeight = 25;

         CheckIsEmpty();
         UpdateShowWatermark();
      }

      new private Id<ReportTextBox> Id => base.Id as Id<ReportTextBox>;

      public string WaterMarkText => Properties.Resources.WaterMarkTextBoxString;

      public string InputText
      {
         get => _inputText;
         set
         {
            if (_inputText == value)
               return;

            _inputText = value;
            RaisePropertyChanged();
         }
      }

      public bool IsEmpty
      {
         get => _isEmpty;
         private set
         {
            if (_isEmpty == value)
               return;

            _isEmpty = value;

            UpdateShowWatermark();
            RaisePropertyChanged();
         }
      }

      public bool IsEditing
      {
         get => _isEditing;
         set
         {
            if (_isEditing == value)
               return;

            _isEditing = value;

            UpdateShowWatermark();
            RaisePropertyChanged();
         }
      }

      public bool ShowWatermark
      {
         get => _showWatermark;
         set
         {
            if (_showWatermark == value)
               return;

            _showWatermark = value;
            RaisePropertyChanged();
         }
      }

      public ICommand StartEditCommand
      {
         get => _startEditCommand;
         private set
         {
            if (_startEditCommand != value)
            {
               _startEditCommand = value;
               RaisePropertyChanged();
            }
         }
      }

      public ICommand EndEditCommand
      {
         get => _endEditCommand;
         private set
         {
            if (_endEditCommand != value)
            {
               _endEditCommand = value;
               RaisePropertyChanged();
            }
         }
      }

      private void OnStartEdit(object obj = null)
      {
         IsEditing = true;
      }

      private void OnEndEdit(object obj = null)
      {
         IsEditing = false;
         SaveTextIfChanged();
      }

      private void SaveTextIfChanged()
      {
         if (IsInputTextChanged())
         {
            _previousInputText = InputText;
            CheckIsEmpty();

            UpdateTextAppState();
         }
      }

      private void UpdateTextAppState()
      {
         _textBoxController.ModifyText(Id, InputText);
      }

      private void CheckIsEmpty()
      {
         IsEmpty = IsTextEmpty(InputText);
      }

      private bool IsInputTextChanged()
      {
         return AreTextsDifferent(InputText, _previousInputText);
      }

      private bool AreTextsDifferent(string text1, string text2)
      {
         if (text1 == text2)
            return false;

         if (IsTextEmpty(text1) && IsTextEmpty(text2))
            return false;

         return true;
      }

      private bool IsTextEmpty(string text)
      {
         return string.IsNullOrWhiteSpace(text) || IsDocumentEmpty(text);
      }

      private bool IsDocumentEmpty(string text)
      {
         var document = GetDocument(text);
         return document.IsEmpty;
      }

      private RadDocument GetDocument(string text)
      {
         var xamlformatProvider = new XamlFormatProvider();
         var doc = xamlformatProvider.Import(text);

         return doc;
      }

      private void UpdateShowWatermark()
      {
         ShowWatermark = IsEmpty && !IsEditing && RenderMode == RenderMode.EditMode;
      }

      protected override void OnUpdate(ISnapShot snapShot)
      {
         base.OnUpdate(snapShot);
         UpdateShowWatermark();
      }

      protected override void UpdateFromBusinessEntity(IReportComponent reportComponentBefore, IReportComponent reportComponentAfter)
      {
         base.UpdateFromBusinessEntity(reportComponentBefore, reportComponentAfter);

         var text = (reportComponentAfter as ReportTextBox).Text;

         if (AreTextsDifferent(text, InputText))
         {
            InputText = text;
            _previousInputText = InputText;

            CheckIsEmpty();
         }
      }

      public override void Dispose()
      {
         StartEditCommand = null;
         EndEditCommand = null;

         base.Dispose();
      }
   }
}
