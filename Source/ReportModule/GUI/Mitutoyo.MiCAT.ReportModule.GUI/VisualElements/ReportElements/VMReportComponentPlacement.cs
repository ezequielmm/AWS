// <copyright file="VMReportComponentPlacement.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements
{
   public class VMReportComponentPlacement : VMResizablePlacement, IVMReportComponentPlacement
   {
      protected readonly IReportComponentPlacementController _placementController;
      protected readonly ISelectedComponentController _selectedComponentController;
      private int _domainX;
      private int _domainY;
      private int _domainWidth;
      private int _domainHeight;

      public VMReportComponentPlacement(IItemId<IReportComponent> reportComponentId,
         ReportComponentPlacement placement,
         IReportComponentPlacementController placementController,
         ISelectedComponentController selectedComponentController)
      {
         ReportComponentId = reportComponentId;
         _placementController = placementController;
         _selectedComponentController = selectedComponentController;
         InitializeFromSnapShot(placement);
      }

      protected virtual void InitializeFromSnapShot(ReportComponentPlacement placement)
      {
         UpdateFromPlacementEntity(null, placement);
      }

      protected IItemId<IReportComponent> ReportComponentId { get; }

      public int DomainX
      {
         get => _domainX;
         protected set
         {
            if (_domainX != value)
            {
               _domainX = value;
               RaisePropertyChanged();
            }
         }
      }

      public int DomainY
      {
         get => _domainY;
         protected set
         {
            if (value != _domainY)
            {
               _domainY = value;
               RaisePropertyChanged();
            }
         }
      }

      public int DomainWidth
      {
         get => _domainWidth;
         protected set
         {
            if (_domainWidth != value)
            {
               _domainWidth = value;
               RaisePropertyChanged();
            }
         }
      }

      public int DomainHeight
      {
         get => _domainHeight;
         protected set
         {
            if (_domainHeight != value)
            {
               _domainHeight = value;
               RaisePropertyChanged();
            }
         }
      }

      public virtual void UpdateFromPlacementEntity(ReportComponentPlacement placementBefore, ReportComponentPlacement placementAfter)
      {
         DomainX = placementAfter.X;
         DomainY = placementAfter.Y;
         DomainWidth = placementAfter.Width;
         DomainHeight = placementAfter.Height;

         ResetVisuals();
      }
      public void UpdateSelectionState(bool newIsSelectedValue)
      {
         IsSelected = newIsSelectedValue;
      }

      private void ResetVisualX()
      {
         VisualX = _domainX;
      }

      protected virtual void ResetVisualY()
      {
         VisualY = _domainY;
      }

      protected virtual void ResetVisualSize()
      {
         if (IsResizable)
         {
            VisualWidth = _domainWidth;
            VisualHeight = _domainHeight;
         }
      }

      protected virtual void ResetVisuals()
      {
         ResetVisualX();
         ResetVisualY();
         ResetVisualSize();
      }

      protected override void SetSelected()
      {
         _selectedComponentController.SetSelected(ReportComponentId);
      }

      public override void CompleteResize()
      {
         base.CompleteResize();
         SetDomainSizeFromVisuals();
      }
      protected override void OnCompleteDrag()
      {
         SetDomainPositionFromVisuals();
      }

      protected virtual void SetDomainSizeFromVisuals()
      {
         _placementController.SetResize(ReportComponentId, VisualX, VisualY, VisualWidth, VisualHeight);
      }
      protected virtual void SetDomainPositionFromVisuals()
      {
         _placementController.SetPosition(ReportComponentId, VisualX, VisualY);
      }
   }
}
