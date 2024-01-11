// <copyright file="ReportBoundarySectionUpdateReceiver.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Clients;
using Mitutoyo.MiCAT.Core;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;

namespace Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection
{
   public class ReportBoundarySectionUpdateReceiver : IReportBoundarySectionUpdateReceiver, IUpdateClient, IDisposable
   {
      private readonly IAppStateHistory _history;
      private IVMReportBoundarySectionComponentFactory _viewModelFactory;
      private List<VMReportBoundarySection> _headerViewModelsList = new List<VMReportBoundarySection>();
      private List<VMReportBoundarySection> _footerViewModelsList = new List<VMReportBoundarySection>();

      public Name Name => nameof(ReportBoundarySectionUpdateReceiver);

      public bool IsInEditMode { get; private set; }
      public bool IsHeaderSectionSelected { get; private set; }
      public bool IsFooterSectionSelected { get; private set; }

      public ReportBoundarySectionUpdateReceiver(IVMReportBoundarySectionComponentFactory viewModelFactory, IAppStateHistory history)
      {
         _viewModelFactory = viewModelFactory;
         _history = history.AddClient(this);
         _history.Subscribe(this, h => h.WithTypes(typeof(ReportModeState), typeof(ReportHeader), typeof(ReportBody), typeof(ReportFooter), typeof(ReportComponentSelection), typeof(ReportSectionSelection), typeof(IReportComponent)));
      }

      public Task Update(ISnapShot snapShot)
      {
         UpdateReportModeState(snapShot);

         UpdateHeader(snapShot);
         UpdateFooter(snapShot);

         return Task.CompletedTask;
      }

      private void UpdateHeader(ISnapShot snapShot)
      {
         var headerState = snapShot.GetReportHeader();
         var isHeaderSelected = snapShot.IsSectionSelected(headerState.Id);
         var newIds = snapShot.GetReportComponentIdsAddedOnContainer(headerState.Id);
         var removedIds = snapShot.GetReportComponentIdsDeletedOnContainer(headerState.Id);

         _headerViewModelsList.ForEach(header => header.SetSectionSelected(isHeaderSelected));
         CreateReportSectionComponentViewModels(_headerViewModelsList, newIds, snapShot);
         DeleteReportSectionComponentViewModels(_headerViewModelsList, removedIds);
      }

      private void UpdateFooter(ISnapShot snapShot)
      {
         var footerState = snapShot.GetReportFooter();
         var isFooterSelected = snapShot.IsSectionSelected(footerState.Id);
         var newIds = snapShot.GetReportComponentIdsAddedOnContainer(footerState.Id);
         var removedIds = snapShot.GetReportComponentIdsDeletedOnContainer(footerState.Id);

         _footerViewModelsList.ForEach(footer => footer.SetSectionSelected(isFooterSelected));
         CreateReportSectionComponentViewModels(_footerViewModelsList, newIds, snapShot);
         DeleteReportSectionComponentViewModels(_footerViewModelsList, removedIds);
      }

      private void UpdateReportModeState(ISnapShot snapShot)
      {
         var isEditModeNow = snapShot.IsReportInEditMode();
         if (IsInEditMode != isEditModeNow)
         {
            IsInEditMode = isEditModeNow;

            _headerViewModelsList.ForEach(header => header.SetReportState(IsInEditMode));
            _footerViewModelsList.ForEach(footer => footer.SetReportState(IsInEditMode));
         }
      }

      public void AddHeaderViewModel(VMReportBoundarySection headerViewModel)
      {
         headerViewModel.SetReportState(IsInEditMode);
         headerViewModel.SetSectionSelected(IsHeaderSectionSelected);
         _headerViewModelsList.Add(headerViewModel);
         CreateReportSectionComponentViewModels(
            new List<VMReportBoundarySection>() { headerViewModel },
            _history.CurrentSnapShot.GetAllReportComponentIdsOnHeader(),
            _history.CurrentSnapShot);
      }

      public void AddFooterViewModel(VMReportBoundarySection footerViewModel)
      {
         footerViewModel.SetReportState(IsInEditMode);
         footerViewModel.SetSectionSelected(IsFooterSectionSelected);
         _footerViewModelsList.Add(footerViewModel);
         CreateReportSectionComponentViewModels(
            new List<VMReportBoundarySection>() { footerViewModel },
            _history.CurrentSnapShot.GetAllReportComponentIdsOnFooter(),
            _history.CurrentSnapShot);
      }

      public void ClearHeaderViewModelsList()
      {
         _headerViewModelsList.ForEach(vm => vm.Dispose());
         _headerViewModelsList.Clear();
      }

      public void ClearFooterViewModelsList()
      {
         _footerViewModelsList.ForEach(vm => vm.Dispose());
         _footerViewModelsList.Clear();
      }

      private void CreateReportSectionComponentViewModels(List<VMReportBoundarySection> reportSections, IEnumerable<IItemId<IReportComponent>> reportComponentIds, ISnapShot snapShot)
      {
         foreach (IItemId<IReportComponent> reportComponentId in reportComponentIds)
         {
            foreach (VMReportBoundarySection reportSection in reportSections)
            {
               var componentViewModel = _viewModelFactory.GetNewViewModel(reportComponentId, snapShot);
               reportSection.AddViewModel(componentViewModel);
            }
         }
      }

      private void DeleteReportSectionComponentViewModels(List<VMReportBoundarySection> reportSections, IEnumerable<IItemId<IReportComponent>> reportComponentIds)
      {
         foreach (IItemId<IReportComponent> reportComponentId in reportComponentIds)
         {
            foreach (VMReportBoundarySection reportSection in reportSections)
            {
               reportSection.RemoveViewModel(reportComponentId);
            }
         }
      }

      public void Dispose()
      {
         ClearHeaderViewModelsList();
         ClearFooterViewModelsList();
         _history.RemoveClient(this);
      }
   }
}
