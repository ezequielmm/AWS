// <copyright file="VMReportBodyPlacement.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements
{
   public class VMReportBodyPlacement : VMReportComponentPlacement, IVMReportBodyPlacement
   {
      private int _pageVerticalOffset;
      private int _extraOffset;

      public event EventHandler<ReportComponentMovedEventArgs> ReportComponentMoved;
      public event EventHandler<ReportComponentLayoutChangedEventArgs> ReportComponentLayoutChanged;

      public VMReportBodyPlacement(IItemId<IReportComponent> reportComponentId,
         ReportComponentPlacement placement,
         IReportComponentPlacementController placementController,
         ISelectedComponentController selectedComponentController,
         IRenderedData renderedData) : base(reportComponentId, placement, placementController, selectedComponentController)
      {
         RenderedData = renderedData;
      }

      protected IRenderedData RenderedData { get; }

      public int PageVerticalOffset
      {
         get => _pageVerticalOffset;
         set
         {
            _pageVerticalOffset = value;
            ResetVisualY();
         }
      }
      public void SetExtraOffset(int extraOffset)
      {
         _extraOffset = extraOffset;
         ResetVisualY();
      }
      public void RaiseReportComponentMoved()
      {
         ReportComponentMoved?.Invoke(this, new ReportComponentMovedEventArgs());
      }

      public void RaiseReportComponentLayoutChanged()
      {
         ReportComponentLayoutChanged?.Invoke(this, new ReportComponentLayoutChangedEventArgs());
      }

      public override void UpdateFromPlacementEntity(ReportComponentPlacement placementBefore, ReportComponentPlacement placementAfter)
      {
         bool wasVerticallyMoved = placementBefore != null && placementBefore.Y != placementAfter.Y;
         bool heightChanged = placementBefore != null && placementBefore.Height != placementAfter.Height;

         DomainX = placementAfter.X;
         DomainY = placementAfter.Y;
         DomainWidth = placementAfter.Width;
         DomainHeight = placementAfter.Height;

         VisualX = DomainX;
         ResetVisualSize();

         if (wasVerticallyMoved)
            RaiseReportComponentMoved();
         else if (heightChanged)
            RaiseReportComponentLayoutChanged();
         else
            ResetVisualY();
      }

      protected override void SetDomainPositionFromVisuals()
      {
         if (IsValidPosition(VisualY))
         {
            var newDomainX = VisualX;
            var newDomainY = ConvertVisualToDomain(VisualY);

            if (newDomainX == DomainX && newDomainY == DomainY)
            {
               ResetVisuals();
            }
            else
            {
               if (IsFakeSpace(VisualY))
               {
                  var currentPage = RenderedData.Pages.GetPageForPosition(VisualY);
                  if (currentPage.PlacementWouldFitOnThisPageOnItsPosition(this))
                     _placementController.SetPositionOnFakeSpace(
                        ReportComponentId,
                        newDomainX,
                        newDomainY,
                        currentPage.EndDomainY,
                        currentPage.GetHeightOfFakeSpace());
                  else
                     ResetVisuals();
               }
               else
               {
                  _placementController.SetPosition(ReportComponentId, newDomainX, newDomainY);
               }
            }
         }
         else
            ResetVisuals();
      }

      protected override void SetDomainSizeFromVisuals()
      {
         if (IsValidPosition(VisualY))
         {
            var newDomainX = VisualX;
            var newDomainY = ConvertVisualToDomain(VisualY);

            if (IsFakeSpace(VisualY))
            {
               var currentPage = RenderedData.Pages.GetPageForPosition(VisualY);
               if (currentPage.PlacementWouldFitOnThisPageOnItsPosition(this))
                  _placementController.SetResizeOnFakeSpace(ReportComponentId, newDomainX, newDomainY, VisualWidth, VisualHeight, currentPage.EndDomainY, currentPage.GetHeightOfFakeSpace());
               else
                  ResetVisuals();
            }
            else
               _placementController.SetResize(ReportComponentId, newDomainX, newDomainY, VisualWidth, VisualHeight);
         }
         else
            ResetVisuals();
      }

      protected int CalculateVisualYFromDomain() => _pageVerticalOffset + DomainY + _extraOffset;

      protected virtual int ConvertVisualToDomain(int visualY)
      {
         return RenderedData.ConvertToDomainY(visualY);
      }

      protected override void ResetVisualY()
      {
         VisualY = CalculateVisualYFromDomain();
      }
      protected virtual bool IsFakeSpace(int visualY)
      {
         return RenderedData.IsFakeSpace(visualY);
      }

      protected virtual bool IsValidPosition(int visualY)
      {
         return !RenderedData.IsInDisabledSpace(visualY);
      }

      protected bool IsMovingDown(int visualY)
      {
         return visualY > CalculateVisualYFromDomain();
      }

      public VMPage GetPage()
      {
         return RenderedData.Pages.GetPageForPosition(VisualY);
      }
      public VMPage GetPageForPlacement(IVMVisualPlacement placement)
      {
         return RenderedData.Pages.GetPageForPosition(placement.VisualY);
      }
   }
}
