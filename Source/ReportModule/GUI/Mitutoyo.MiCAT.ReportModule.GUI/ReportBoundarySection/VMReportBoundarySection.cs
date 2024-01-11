// <copyright file="VMReportBoundarySection.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection.ValueConverters;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection
{
   public class VMReportBoundarySection : VMBase, IDisposable
   {
      private int _height;
      private bool _isOnEditMode;
      private bool _isSectionOnEditMode;
      private bool _isOverlayVisible;
      private bool _isSectionSelected = false;
      private ObservableCollection<IVMReportComponent> _componentViewModels = new ObservableCollection<IVMReportComponent>();
      private readonly IItemId _reportBoundarySectionId;
      protected IImageController _imageController;
      protected ITextBoxController _textboxController;
      protected IHeaderFormController _headerFormController;
      protected ISelectedComponentController _componentSelectionController;
      protected ISelectedSectionController _sectionSelectionController;

      public VMReportBoundarySection(IItemId reportBoundarySectionId, int height, int pageNumber, IImageController imageController, ITextBoxController textboxController, IHeaderFormController headerFormController, ISelectedComponentController componentSelectionController, ISelectedSectionController sectionSelectionController)
      {
         _reportBoundarySectionId = reportBoundarySectionId;
         Height = height;
         PageNumber = pageNumber;
         _imageController = imageController;
         _textboxController = textboxController;
         _headerFormController = headerFormController;
         _componentSelectionController = componentSelectionController;
         _sectionSelectionController = sectionSelectionController;

         AddImageCommand = new RelayCommand<ContextMenuCommandArgs>(OnAddImage);
         AddHeaderFormCommand = new RelayCommand<ContextMenuCommandArgs>(OnAddHeaderForm);
         AddTextboxCommand = new RelayCommand<ContextMenuCommandArgs>(OnAddTextbox);
         SelectReportBoundarySection = new RelayCommand<object>(OnReportBoundarySectionSelected);
      }

      public ObservableCollection<IVMReportComponent> ComponentViewModels
      {
         get { return _componentViewModels; }
         private set
         {
            _componentViewModels = value;
            RaisePropertyChanged();
         }
      }

      public ICommand AddImageCommand { get; set; }
      public ICommand AddHeaderFormCommand { get; set; }
      public ICommand AddTextboxCommand { get; set; }
      public ICommand SelectReportBoundarySection { get; set; }

      public int PageNumber { get; protected set; }

      public bool IsSectionOnEditMode
      {
         get
         {
            return _isSectionOnEditMode;
         }
         private set
         {
            if (_isSectionOnEditMode == value)
               return;

            _isSectionOnEditMode = value;
            RaisePropertyChanged();
         }
      }
      public bool IsOverlayVisible
      {
         get
         {
            return _isOverlayVisible;
         }
         private set
         {
            if (_isOverlayVisible == value)
               return;

            _isOverlayVisible = value;
            RaisePropertyChanged();
         }
      }

      public int Height
      {
         get
         {
            return _height;
         }
         set
         {
            if (_height == value)
               return;

            _height = value;
            RaisePropertyChanged();
         }
      }

      public void AddViewModel(IVMReportComponent componentViewModel)
      {
         ComponentViewModels.Add(componentViewModel);
      }

      public void RemoveViewModel(IItemId<IReportComponent> reportComponentId)
      {
         var viewModelToRemove = ComponentViewModels.Where(vm => vm.Id.Equals(reportComponentId)).FirstOrDefault();
         if (viewModelToRemove != null)
         {
            viewModelToRemove.Dispose();
            ComponentViewModels.Remove(viewModelToRemove);
         }
      }

      public void SetSectionSelected(bool isSectionSelected)
      {
         if (_isSectionSelected != isSectionSelected)
         {
            _isSectionSelected = isSectionSelected;
            UpdateIsSectionOnEditMode();
         }
      }

      public void SetReportState(bool isOnEditMode)
      {
         if (_isOnEditMode != isOnEditMode)
         {
            _isOnEditMode = isOnEditMode;
            UpdateIsSectionOnEditMode();
         }
      }

      private void OnAddImage(ContextMenuCommandArgs arg)
      {
         _imageController.AddImageToBoundarySection(_reportBoundarySectionId, (int)arg.Position.X, (int)arg.Position.Y);
      }

      private void OnAddTextbox(ContextMenuCommandArgs arg)
      {
         _textboxController.AddTextboxToSection(_reportBoundarySectionId, (int)arg.Position.X, (int)arg.Position.Y);
      }

      private void OnAddHeaderForm(ContextMenuCommandArgs arg)
      {
         _headerFormController.AddHeaderFormToSection(_reportBoundarySectionId, (int)arg.Position.X, (int)arg.Position.Y);
      }

      private void OnReportBoundarySectionSelected(object obj)
      {
         _sectionSelectionController.SelectReportBoundarySections();
      }

      private void UpdateIsSectionOnEditMode()
      {
         IsSectionOnEditMode = _isSectionSelected && _isOnEditMode;
         IsOverlayVisible = !_isSectionSelected && _isOnEditMode;
      }

      public void Dispose()
      {
         IsSectionOnEditMode = false;

         foreach (var vmComponent in _componentViewModels)
            vmComponent.Dispose();
      }
   }
}
