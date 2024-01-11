// <copyright file="VMReportBoundarySectionComponentFactory.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Image.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.TextBox.ViewModels;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements
{
   public class VMReportBoundarySectionComponentFactory : IVMReportBoundarySectionComponentFactory
   {
      private IAppStateHistory _appStateHistory;
      private IReportComponentPlacementController _placementController;
      private IImageController _imageController;
      private ITextBoxController _textboxController;
      private IHeaderFormController _headerFormController;
      private IHeaderFormFieldController _headerFormFieldController;
      private ISelectedComponentController _selectionController;
      private IDeleteComponentController _deleteComponentController;
      private IActionCaller _actionCaller;

      public VMReportBoundarySectionComponentFactory(
         IAppStateHistory appStateHistory,
         IReportComponentPlacementController placementController,
         IImageController imageController,
         ITextBoxController textboxController,
         IHeaderFormController headerFormController,
         IHeaderFormFieldController headerFormFieldController,
         ISelectedComponentController selectionController,
         IDeleteComponentController deleteComponentController,
         IActionCaller actionCaller)
      {
         _appStateHistory = appStateHistory;
         _placementController = placementController;
         _imageController = imageController;
         _textboxController = textboxController;
         _headerFormController = headerFormController;
         _headerFormFieldController = headerFormFieldController;
         _selectionController = selectionController;
         _actionCaller = actionCaller;
         _deleteComponentController = deleteComponentController;
      }
      public IVMReportComponent GetNewViewModel(IItemId<IReportComponent> reportComponentId, ISnapShot snapShot)
      {
         IReportComponent reportComponentAdded = snapShot.GetItem(reportComponentId);
         VMReportComponentPlacement placementForTheComponentAdded = new VMReportComponentPlacement(reportComponentId, reportComponentAdded.Placement, _placementController, _selectionController);

         switch (reportComponentAdded)
         {
            case ReportImage reportImage:
               return new VMImage(_appStateHistory, reportImage.Id, placementForTheComponentAdded, _deleteComponentController, _imageController, _actionCaller);
            case ReportTextBox reportTextBox:
               return new VMTextBox(_appStateHistory, reportTextBox.Id, placementForTheComponentAdded, _deleteComponentController, _textboxController);
            case ReportHeaderForm reportHeaderForm:
               return new VMHeaderForm(_appStateHistory, reportHeaderForm.Id, placementForTheComponentAdded, _deleteComponentController, _headerFormController, _headerFormFieldController);
            default:
               throw new Exception($"Not supported component: {reportComponentId.ItemType.Name}");
         }
      }
   }
}
