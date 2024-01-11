// <copyright file="VMPage.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Linq;
using System.Windows;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;

namespace Mitutoyo.MiCAT.ReportModule.GUI.PageLayout
{
   public class VMPage : VMBase
   {
      public const int SPACE_BETWEEN_PAGES = 20;
      private int _startVisualY;
      private int _offsetForComponentsOnThisPage;
      private int _endDomainY;
      private int _endDomainYAsVisual;
      private int _endDomainYasYRelativeToPage;
      public VMReportBoundarySection _header;
      public VMReportBoundarySection _footer;

      public VMPage(CommonPageLayout pageLayout, IReportModeProperty reportModeProperty, VMReportBoundarySection header, VMReportBoundarySection footer)
      {
         Height = pageLayout.PageSize.Height;
         Width = pageLayout.PageSize.Width;
         Margins = pageLayout.CanvasMargin;
         ReservedUpperSpace = pageLayout.GetUpperSpace();
         ReservedBottomSpace = pageLayout.GetBottomSpace();

         Header = header;
         Footer = footer;
         ReportModeProperty = reportModeProperty;

         VisualizationPagesSeparation = new Thickness(0, SPACE_BETWEEN_PAGES, 0, 0);

         TopMargin = new VMMargin(Margins.Top, Width, reportModeProperty);
         BottomMargin = new VMMargin(Height - Margins.Bottom, Width, reportModeProperty);

         SetEndVisualYForThisStartVisualY();
      }
      public IReportModeProperty ReportModeProperty { get; }
      public VMMargin TopMargin{ get; set; }
      public VMMargin BottomMargin { get; set; }
      public VMReportBoundarySection Header
      {
         get { return _header; }
         protected set
         {
            _header = value; RaisePropertyChanged();
         }
      }
      public VMReportBoundarySection Footer
      {
         get { return _footer; }
         protected set
         {
            _footer = value; RaisePropertyChanged();
         }
      }
      public Thickness VisualizationPagesSeparation { get; }

      public Margin Margins { get; }

      public int Height { get; }
      public int Width { get; }
      public int ReservedUpperSpace { get; }
      public int ReservedBottomSpace { get; }

      public bool PageDebugInfoEnabled
      {
         get { return Environment.GetCommandLineArgs().Contains("debug"); } //Refactor: Is just for internal use.
      }

      public int StartVisualY
      {
         get
         {
            return _startVisualY;
         }
         set
         {
            if (_startVisualY == value)
               return;

            _startVisualY = value;

            SetEndVisualYForThisStartVisualY();
         }
      }

      public int EndVisualY { get; private set; }
      public int StartDomainY { get; set; }

      public int EndDomainY
      {
         get => _endDomainY;
         private set
         {
            if (_endDomainY == value)
               return;

            _endDomainY = value;
         }
      }

      public int EndDomainYAsVisual
      {
         get => _endDomainYAsVisual;
         private set
         {
            if (value == _endDomainYAsVisual)
               return;

            _endDomainYAsVisual = value;
            SetEndDomainYRelativeToPage();
         }
      }

      public int OffsetForComponentsOnThisPage
      {
         get => _offsetForComponentsOnThisPage;
         set
         {
            if (_offsetForComponentsOnThisPage == value)
               return;

            _offsetForComponentsOnThisPage = value;

            SetEndDomainYRelativeToPage();
         }
      }

      public int GetVisuaYFromYRelativeToThisPage(int yRelativeOnThisPage)
      {
         return StartVisualY - ReservedUpperSpace + yRelativeOnThisPage;
      }

      public void SetDefaultEndDomainYForThisStartDomainY()
      {
         EndDomainY = StartDomainY + UsableSpaceHeight() - 1;
      }

      private void SetEndVisualYForThisStartVisualY()
      {
         EndVisualY = StartVisualY + UsableSpaceHeight() - 1;
         EndDomainYAsVisual = EndVisualY;
      }

      public void SetPageBreakInfoByComponent(IVMReportComponentPlacement placementJumpingPage)
      {
         EndDomainY = placementJumpingPage.DomainY - 1;
         EndDomainYAsVisual = placementJumpingPage.VisualY - 1;
      }
      public void SetValuesAffectedByDisabledSpace(DisabledSpaceData disabledSpace)
      {
         if (IsDisabledSpaceAffectingJustThisPage(disabledSpace))
         {
            EndDomainY -= disabledSpace.UsableSpaceTaken;
         }
         else if (IsDisabledSpaceStartingOnThisPage(disabledSpace))
         {
            EndDomainY = disabledSpace.StartDomainY;
         }
         else if (IsDisabledSpaceEndingOnThisPage(disabledSpace))
         {
            int pixelsOfDisabledSpaceBeginingThisPage = disabledSpace.EndVisualY - StartVisualY;

            StartDomainY = disabledSpace.StartDomainY;
            EndDomainY = disabledSpace.StartDomainY + UsableSpaceHeight() - pixelsOfDisabledSpaceBeginingThisPage - 1;
         }
         else if (IsThisPageWithinDisabledSpace(disabledSpace))
         {
            StartDomainY = disabledSpace.StartDomainY;
            EndDomainY = StartDomainY;
         }
         else if (IsBellowOfDisabledSpace(disabledSpace))
         {
            StartDomainY -= disabledSpace.UsableSpaceTaken;
            EndDomainY -= disabledSpace.UsableSpaceTaken;
         }
      }
      private bool IsDisabledSpaceAffectingJustThisPage(DisabledSpaceData disabledSpace)
      {
         return this == disabledSpace.StartPage && this == disabledSpace.EndPage;
      }
      private bool IsDisabledSpaceStartingOnThisPage(DisabledSpaceData disabledSpace)
      {
         return this == disabledSpace.StartPage;
      }
      private bool IsDisabledSpaceEndingOnThisPage(DisabledSpaceData disabledSpace)
      {
         return this == disabledSpace.EndPage;
      }
      private bool IsThisPageWithinDisabledSpace(DisabledSpaceData disabledSpace)
      {
         return disabledSpace.StartVisualY < StartVisualY && disabledSpace.EndVisualY > EndVisualY;
      }
      private bool IsBellowOfDisabledSpace(DisabledSpaceData disabledSpace)
      {
         return EndVisualY > disabledSpace.StartVisualY;
      }

      public int UsableSpaceHeight()
      {
         return Height - ReservedUpperSpace - ReservedBottomSpace;
      }

      public bool ReportElementWouldFitOnThisPage(IVMReportComponent element)
      {
         return element.VMPlacement.DomainHeight <= UsableSpaceHeight();
      }

      public bool PlacementWouldFitOnThisPageOnItsPosition(IVMReportComponentPlacement placement)
      {
         return (ExcedentSpaceToFitOnThisPageOnItsPosition(placement) == 0);
      }

      public int ExcedentSpaceToFitOnThisPageOnItsPosition(IVMReportComponentPlacement placement)
      {
         var result = (placement.VisualY + placement.DomainHeight) - EndVisualY;

         if (result < 0)
            return 0;
         else
            return result;
      }

      public int SpaceLeftOnThisPage(int visualY)
      {
         if (visualY > EndVisualY)
            return 0;
         else
            return (EndVisualY - visualY);
      }

      public bool YVisualPositionIsOnThisPage(int yVisualPosition)
      {
         return (yVisualPosition >= StartVisualY) && (yVisualPosition <= EndVisualY);
      }

      public int GetHeightOfFakeSpace()
      {
         return (EndVisualY - EndDomainYAsVisual);
      }

      public bool IsFakeSpace(int visualy)
      {
         return (visualy > EndDomainYAsVisual && visualy < EndVisualY);
      }

      // Debug
      private void SetEndDomainYRelativeToPage()
      {
         EndDomainYasYRelativeToPage = EndDomainYAsVisual - _startVisualY + ReservedUpperSpace;
      }

      public int EndDomainYasYRelativeToPage
      {
         get => _endDomainYasYRelativeToPage;
         set
         {
            if (_endDomainYasYRelativeToPage == value)
               return;

            _endDomainYasYRelativeToPage = value;

            RaisePropertyChanged();
         }
      }
   }
}
