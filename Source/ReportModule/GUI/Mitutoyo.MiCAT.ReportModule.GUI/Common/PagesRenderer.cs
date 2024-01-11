// <copyright file="PagesRenderer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Clients;
using Mitutoyo.MiCAT.Core;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElementList.ViewModels;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common
{
   public class PagesRenderer : VMBase, IPagesRenderer, IUpdateClient, IDisposable
   {
      private readonly IAppStateHistory _appStateHistory;
      private CommonPageLayout _actualPageSettings;
      private IPageLayoutCalculator _pageLayoutCalculator;
      private IActionCaller _actionCaller;

      //REFACTOR_DERIVE_APPSTATE_CHANGE: Used to refactor values shared between VMs that should be added to the snapshot. Maybe we need to implement ControllerRunner before these refactors.
      public event EventHandler<LayoutRecalculationFinishEventArgs> LayoutRecalculationFinished;

      public PagesRenderer(IRenderedData renderedData, IVMReportElementList elementList, IPageLayoutCalculator pageLayoutCalculator, IAppStateHistory appStateHistory, IActionCaller actionCaller)
      {
         RenderedData = renderedData;
         ElementList = elementList;
         ElementList.LayoutRecalculationNeeded += ReportElementList_LayoutRecalculationNeeded;

         _actionCaller = actionCaller;
         _pageLayoutCalculator = pageLayoutCalculator;
         _appStateHistory = appStateHistory.AddClient(this);
         var appStateSubscription = new Subscription(this).With(new TypeFilter(typeof(CommonPageLayout)));
         _appStateHistory.Subscribe(appStateSubscription);
      }
      protected void RaiseLayoutRecalculationFinish()
      {
         LayoutRecalculationFinished?.Invoke(this, new LayoutRecalculationFinishEventArgs());
      }

      private void ReportElementList_LayoutRecalculationNeeded(object sender, LayoutRecalculationNeededEventArgs e)
      {
         RecalculateVisualPositions();
      }

      public IRenderedData RenderedData { get; }
      public IVMReportElementList ElementList { get; }
      public CommonPageLayout ActualPageSettings
      {
         get { return _actualPageSettings; }
         private set
         {
            _actualPageSettings = value;
            RaisePropertyChanged();
         }
      }

      public Name Name => nameof(PageLayoutCalculator);

      public Task Update(ISnapShot snapShot)
      {
         UpdateFromCommonPageLayoutSettings(snapShot);

         return Task.CompletedTask;
      }

      private void UpdateFromCommonPageLayoutSettings(ISnapShot snapShot)
      {
         var commonPageLayout = snapShot.GetCommonPageLayout();

         if (!commonPageLayout.Equals(ActualPageSettings))
         {
            ActualPageSettings = commonPageLayout;
            RecalculateVisualPositions();
         }
      }

      private void RecalculateVisualPositions()
      {
         _actionCaller.RunUIThreadAction(RecalculateComponentPositions);
      }

      private void RecalculateComponentPositions()
      {
         _pageLayoutCalculator.CalculateComponentPositions(ActualPageSettings, ElementList, RenderedData);
         RaiseLayoutRecalculationFinish(); //REFACTOR_DERIVE_APPSTATE_CHANGE: Used to refactor values shared between VMs that should be added to the snapshot. Maybe we need to implement ControllerRunner before these refactors.
      }

      public void Dispose()
      {
         _appStateHistory.RemoveClient(this);
      }
   }
}
