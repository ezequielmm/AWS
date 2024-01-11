// <copyright file="VMReportBoundarySectionFactory.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection
{
   public class VMReportBoundarySectionFactory
   {
      private readonly IAppStateHistory _appStateHistory;
      private readonly IReportBoundarySectionUpdateReceiver _updateReceiver;
      private readonly IImageController _imageController;
      private readonly ITextBoxController _textboxController;
      private readonly IHeaderFormController _headerFormController;
      private readonly ISelectedComponentController _componentSelectionController;
      private readonly ISelectedSectionController _sectionSelectionController;

      public VMReportBoundarySectionFactory(IAppStateHistory appStateHistory, IReportBoundarySectionUpdateReceiver updateReceiver, IImageController imageController, ITextBoxController textboxController, IHeaderFormController headerFormController, ISelectedComponentController componentSelectionController, ISelectedSectionController sectionSelectionController)
      {
         _appStateHistory = appStateHistory;
         _updateReceiver = updateReceiver;
         _imageController = imageController;
         _textboxController = textboxController;
         _headerFormController = headerFormController;
         _componentSelectionController = componentSelectionController;
         _sectionSelectionController = sectionSelectionController;
      }

      public VMReportBoundarySection CreateForFooter(int height, int pageNumber)
      {
         IItemId reportBoundarySectionId = _appStateHistory.CurrentSnapShot.GetReportFooterId();
         var vmReportBoundarySection = CreateVMReportBoundarySection(reportBoundarySectionId, height, pageNumber);

         _updateReceiver.AddFooterViewModel(vmReportBoundarySection);

         return vmReportBoundarySection;
      }

      public VMReportBoundarySection CreateForHeader(int height, int pageNumber)
      {
         IItemId reportBoundarySectionId = _appStateHistory.CurrentSnapShot.GetReportHeaderId();
         var vmReportBoundarySection = CreateVMReportBoundarySection(reportBoundarySectionId, height, pageNumber);

         _updateReceiver.AddHeaderViewModel(vmReportBoundarySection);

         return vmReportBoundarySection;
      }

      private VMReportBoundarySection CreateVMReportBoundarySection(IItemId reportBoundarySectionId, int height, int pageNumber)
      {
         return new VMReportBoundarySection(reportBoundarySectionId, height, pageNumber, _imageController, _textboxController, _headerFormController, _componentSelectionController, _sectionSelectionController);
      }
   }
}
