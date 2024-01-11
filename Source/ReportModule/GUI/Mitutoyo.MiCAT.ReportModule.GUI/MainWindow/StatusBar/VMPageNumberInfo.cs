// <copyright file="VMPageNumberInfo.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Clients;
using Mitutoyo.MiCAT.Core;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.StatusBar;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;

namespace Mitutoyo.MiCAT.ReportModule.GUI
{
   public class VMPageNumberInfo : VMBase, IVMPageNumberInfo, IUpdateClient, IDisposable
   {
      private int _currentPage = 1;
      private int _totalPage = 1;
      private string _pageNumberInfo;
      private readonly IPagesRenderer _pagesRenderer;
      private readonly IAppStateHistory _appStateHistory;
      public VMPageNumberInfo(IAppStateHistory appStateHistory, IPagesRenderer pagesRenderer)
      {
         _pagesRenderer = pagesRenderer;
         _appStateHistory = appStateHistory.AddClient(this);
         var appStateSubscription = new Subscription(this).With(new TypeFilter(typeof(ReportComponentSelection)));
         _appStateHistory.Subscribe(appStateSubscription);

         _pagesRenderer.LayoutRecalculationFinished += ReportElementList_LayoutRecalculationFinish; //REFACTOR_DERIVE_APPSTATE_CHANGE: We would need to get the actual number page for a selected component and total pages from the appstate
         UpdatePageNumberInfo();
      }
      private void ReportElementList_LayoutRecalculationFinish(object sender, LayoutRecalculationFinishEventArgs e)
      {
         UpdatePageNumbers();
         TotalPage = _pagesRenderer.RenderedData.Pages.Pages.Count;
      }
      private void UpdatePageNumbers()
      {
         if (SelectedReportComponentId != null)
         {
            var component = _pagesRenderer.ElementList.Elements.OfType<IVMReportComponent>().FirstOrDefault(c => c.Id.Equals(_selectedReportComponentId));
            if (component != null)
            {
               var pageComp = _pagesRenderer.RenderedData.Pages.GetPageForPosition(component.VMPlacement.VisualY);
               CurrentPage = _pagesRenderer.RenderedData.Pages.Pages.IndexOf(pageComp) + 1;
            }
         }
         else
            CurrentPage = 0;
      }

      private IItemId _selectedReportComponentId;
      private IItemId SelectedReportComponentId
      {
         get { return _selectedReportComponentId; }
         set
         {
            _selectedReportComponentId = value;
            UpdatePageNumbers();
         }
      }

      public string PageNumberInfo
      {
         get { return _pageNumberInfo; }
         private set
         {
            if (_pageNumberInfo != value)
            {
               _pageNumberInfo = value;
               RaisePropertyChanged();
            }
         }
      }
      public int CurrentPage
      {
         get { return _currentPage; }
         set
         {
            _currentPage = value;
            UpdatePageNumberInfo();
         }
      }
      public int TotalPage
      {
         get { return _totalPage; }
         set
         {
            _totalPage = value;
            UpdatePageNumberInfo();
         }
      }

      public Name Name => nameof(VMPageNumberInfo);

      private void UpdatePageNumberInfo()
      {
         if (CurrentPage != 0)
         {
            PageNumberInfo = string.Format(Resources.PageString, CurrentPage, TotalPage);
         }
         else
         {
            PageNumberInfo = string.Format(Resources.PageString, "#", TotalPage);
         }
      }

      public Task Update(ISnapShot snapShot)
      {
         UpdateSelectedComponent(snapShot);
         return Task.CompletedTask;
      }

      private void UpdateSelectedComponent(ISnapShot snapShot)
      {
         var reportComponentSelection = snapShot.GetItems<ReportComponentSelection>().Single();

         SelectedReportComponentId = reportComponentSelection.SelectedReportComponentIds.LastOrDefault();
      }

      public void Dispose()
      {
         _appStateHistory.RemoveClient(this);
      }
   }
}