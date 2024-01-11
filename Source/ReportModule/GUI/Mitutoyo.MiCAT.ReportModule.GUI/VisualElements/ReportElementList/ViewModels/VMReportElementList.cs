// <copyright file="VMReportElementList.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Clients;
using Mitutoyo.MiCAT.Core;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Image.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.TesselationView.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.TextBox.ViewModels;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElementList.ViewModels
{
   public class VMReportElementList : VMBase, IUpdateClient, IVMReportElementList, IDisposable
   {
      private readonly IAppStateHistory _appStateHistory;
      private readonly IReportComponentPlacementController _placementController;
      private readonly ITextBoxController _textBoxController;
      private readonly ITableViewController _tableViewController;
      private readonly ITessellationViewController _tessellationViewController;
      private readonly IImageController _imageController;
      private readonly IHeaderFormController _headerFormController;
      private readonly IHeaderFormFieldController _headerFormFieldController;
      private readonly ISelectedComponentController _selectedComponentController;
      private readonly IDeleteComponentController _deleteComponentController;
      private readonly IActionCaller _actionCaller;
      private readonly IRenderedData _renderedData;

      public ImmutableList<IVMReportComponent> _elements;
      public IImmutableList<IVMVisualElementPiece> _elementPieces;

      public VMReportElementList(
         IRenderedData renderedData,
         IAppStateHistory appStateHistory,
         IReportComponentPlacementController placementController,
         ITextBoxController textBoxController,
         ITableViewController tableViewController,
         ITessellationViewController tessellationViewController,
         IImageController imageController,
         IHeaderFormController headerFormController,
         IHeaderFormFieldController headerFormFieldController,
         ISelectedComponentController selectedComponentController,
         IDeleteComponentController deleteComponentController,
         IActionCaller actionCaller)
      {
         _appStateHistory = appStateHistory.AddClient(this);
         _appStateHistory.Subscribe(this, b => b.WithType<IReportComponent>());

         _renderedData = renderedData;

         _placementController = placementController;
         _textBoxController = textBoxController;
         _tableViewController = tableViewController;
         _tessellationViewController = tessellationViewController;
         _imageController = imageController;
         _headerFormController = headerFormController;
         _headerFormFieldController = headerFormFieldController;
         _selectedComponentController = selectedComponentController;
         _deleteComponentController = deleteComponentController;
         _actionCaller = actionCaller;
         Elements = new List<IVMReportComponent>().ToImmutableList();
      }

      public event EventHandler<LayoutRecalculationNeededEventArgs> LayoutRecalculationNeeded;
      protected void RaiseLayoutRecalculationNeeded()
      {
         LayoutRecalculationNeeded?.Invoke(this, new LayoutRecalculationNeededEventArgs());
      }

      public ImmutableList<IVMReportComponent> Elements
      {
         get { return _elements; }
         private set
         {
            _elements = value;
            RaiseLayoutRecalculationNeeded();
            RaisePropertyChanged();
         }
      }

      public IImmutableList<IVMVisualElementPiece> ElementPieces
      {
         get { return _elementPieces; }
         private set
         {
            _elementPieces = value;
            RaisePropertyChanged();
         }
      }

      public Name Name => nameof(VMReportElementList);

      public void RemovePieces(IImmutableList<IVMVisualElementPiece> piecesToRemove)
      {
         if ((piecesToRemove != null) && (ElementPieces != null))
            ElementPieces = ElementPieces.Except(piecesToRemove).ToImmutableList();
      }
      public void AddPieces(IImmutableList<IVMVisualElementPiece> piecesToAdd)
      {
         if (piecesToAdd != null)
         {
            if (ElementPieces == null)
               ElementPieces = piecesToAdd;
            else
               ElementPieces = ElementPieces.AddRange(piecesToAdd);
         }
      }

      public Task Update(ISnapShot snapShot)
      {
         UpdateReportComponentOnList(snapShot);

         return Task.CompletedTask;
      }

      private void UpdateReportComponentOnList(ISnapShot snapShot)
      {
         bool hasChange = false;

         var newList = new List<IVMReportComponent>(_elements);
         var vmsToAdd = ComponentsToAdd(snapShot);

         if (vmsToAdd.Any())
         {
            newList.AddRange(vmsToAdd);
            hasChange = true;
         }

         if (RemoveComponentsFromList(newList, snapShot.GetAllReportComponentIdsDeletedOnBody()))
            hasChange = true;

         if (hasChange)
         {
            Elements = newList.ToImmutableList();
         }
      }

      private IEnumerable<IVMReportComponent> ComponentsToAdd(ISnapShot snapShot)
      {
         List<IVMReportComponent> result = new List<IVMReportComponent>();

         foreach (var reportComponentIdAdded in snapShot.GetAllReportComponentIdsAddedOnBody())
         {
            IVMReportComponent vmComponentAdded = null;
            IVMReportBodyPlacement vmPlacementForTheComponentAdded = null;
            IReportComponent reportComponent = snapShot.GetItem(reportComponentIdAdded);

            switch (reportComponent)
            {
               case ReportImage reportImage:
                  vmPlacementForTheComponentAdded = new VMReportBodyPlacement(reportComponentIdAdded, reportComponent.Placement, _placementController, _selectedComponentController, _renderedData);
                  vmComponentAdded = new VMImage(_appStateHistory, reportImage.Id, vmPlacementForTheComponentAdded, _deleteComponentController, _imageController, _actionCaller);
                  break;
               case ReportTableView reportTable:
                  vmPlacementForTheComponentAdded = new VMDisableSpacePlacement(reportComponentIdAdded, reportComponent.Placement, _placementController, _selectedComponentController, _renderedData);
                  vmComponentAdded = new VMTable(_appStateHistory, reportTable.Id, vmPlacementForTheComponentAdded as VMDisableSpacePlacement, _deleteComponentController, _tableViewController);
                  break;
               case ReportTextBox reportTextBox:
                  vmPlacementForTheComponentAdded = new VMReportBodyPlacement(reportComponentIdAdded, reportComponent.Placement, _placementController, _selectedComponentController, _renderedData);
                  vmComponentAdded = new VMTextBox(_appStateHistory, reportTextBox.Id, vmPlacementForTheComponentAdded,  _deleteComponentController, _textBoxController);
                  break;
               case ReportTessellationView reportTessellation:
                  vmPlacementForTheComponentAdded = new VMReportBodyPlacement(reportComponentIdAdded, reportComponent.Placement, _placementController, _selectedComponentController, _renderedData);
                  vmComponentAdded = new VMTessellationView(_appStateHistory, reportTessellation.Id, vmPlacementForTheComponentAdded, _deleteComponentController, _tessellationViewController);
                  break;
               case ReportHeaderForm reportHeaderForm:
                  vmPlacementForTheComponentAdded = new VMReportBodyPlacement(reportComponentIdAdded, reportComponent.Placement, _placementController, _selectedComponentController, _renderedData);
                  vmComponentAdded = new VMHeaderForm(_appStateHistory, reportHeaderForm.Id, vmPlacementForTheComponentAdded, _deleteComponentController, _headerFormController, _headerFormFieldController);
                  break;
            }

            if (vmComponentAdded != null)
            {
               result.Add(vmComponentAdded);
               vmPlacementForTheComponentAdded.ReportComponentMoved += AnyReportComponentMoved;
               vmPlacementForTheComponentAdded.ReportComponentLayoutChanged += AnyReportComponentLayoutChanged;
            }
         }

         return result;
      }

      private void AnyReportComponentLayoutChanged(object sender, ReportComponentLayoutChangedEventArgs e)
      {
         RaiseLayoutRecalculationNeeded();
      }
      private void AnyReportComponentMoved(object sender, ReportComponentMovedEventArgs e)
      {
         RaiseLayoutRecalculationNeeded();
      }

      private bool RemoveComponentsFromList(List<IVMReportComponent> actualList, IEnumerable<IItemId<IReportComponent>> deletedReportComponentsIds)
      {
         bool anyDeleted = false;

         foreach (var reportComponentDeleted in deletedReportComponentsIds)
         {
            var reportElementToDelete = actualList.FirstOrDefault(x => x.Id.Equals(reportComponentDeleted));

            actualList.Remove(reportElementToDelete);
            anyDeleted = true;
            if (reportElementToDelete.VMPlacement is VMReportBodyPlacement bodyPlacement)
            {
               bodyPlacement.ReportComponentMoved -= AnyReportComponentMoved;
               bodyPlacement.ReportComponentLayoutChanged -= AnyReportComponentLayoutChanged;
            }

            if (reportElementToDelete is IVMMultiPageSplittableElement multiPageElement)
               RemovePieces(multiPageElement.Pieces);

            reportElementToDelete.Dispose();
         }
         return anyDeleted;
      }

      public void Dispose()
      {
         _appStateHistory.RemoveClient(this);
         foreach (var reportComponent in Elements.OfType<IDisposable>())
         {
            reportComponent.Dispose();
         }
      }
   }
}
