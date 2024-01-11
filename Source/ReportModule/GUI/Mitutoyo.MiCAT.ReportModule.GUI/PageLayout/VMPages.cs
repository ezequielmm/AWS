// <copyright file="VMPages.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>
using System.Collections.ObjectModel;
using System.Linq;
using Mitutoyo.MiCAT.Core.StdLib.Extensions;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace;
using Mitutoyo.MiCAT.ReportModule.GUI.ReportBoundarySection;

namespace Mitutoyo.MiCAT.ReportModule.GUI.PageLayout
{
   public class VMPages : VMBase, IVMPages
   {
      private ObservableCollection<VMPage> _pages;
      private int _totalPagesHeight;
      private VMReportBoundarySectionFactory _reportBoundarySectionFactory;
      private IReportBoundarySectionUpdateReceiver _reportBoundarySectionUpdateReceiver;
      private IReportModeProperty _reportModeProperty;

      public VMPages(VMReportBoundarySectionFactory reportBoundarySectionFactory, IReportBoundarySectionUpdateReceiver reportBoundarySectionUpdateReceiver,
         IReportModeProperty reportModeProperty) : this(new ObservableCollection<VMPage>())
      {
         _reportBoundarySectionFactory = reportBoundarySectionFactory;
         _reportBoundarySectionUpdateReceiver = reportBoundarySectionUpdateReceiver;
         _reportModeProperty = reportModeProperty;
      }
      private VMPages(ObservableCollection<VMPage> pages)
      {
         _pages = pages;
      }

      public ObservableCollection<VMPage> Pages
      {
         get { return _pages; }
      }

      public int TotalPagesHeight
      {
         get { return _totalPagesHeight; }
         private set
         {
            _totalPagesHeight = value;
            RaisePropertyChanged();
         }
      }

      public void ClearPages()
      {
         _reportBoundarySectionUpdateReceiver.ClearHeaderViewModelsList();
         _reportBoundarySectionUpdateReceiver.ClearFooterViewModelsList();
         Pages.Clear();
      }

      public int GetDomainOutsidePages(int visualY)
      {
         if (Pages.Count > 0)
         {
            if (visualY < Pages[0].StartVisualY)
               return Pages[0].StartDomainY;
            else
            {
               for (int i = 0; i < (Pages.Count - 1); i++)
               {
                  if (visualY >= Pages[i].EndVisualY && visualY < Pages[i + 1].StartVisualY)
                     return Pages[i + 1].StartDomainY;
               }
               return Pages.Last().EndDomainY + 1;
            }
         }
         else
            return visualY;
      }

      public VMPage GetPageForPosition(int visualY)
      {
         return Pages.FirstOrDefault(p => p.StartVisualY <= visualY && p.EndVisualY > visualY);
      }

      public int VisualOffsetFirstPage(CommonPageLayout actualPageSettings)
      {
         return VMPage.SPACE_BETWEEN_PAGES + actualPageSettings.GetUpperSpace();
      }

      public int VisualOffsetPerPage(CommonPageLayout actualPageSettings)
      {
         return actualPageSettings.GetBottomSpace() + VMPage.SPACE_BETWEEN_PAGES + actualPageSettings.GetUpperSpace();
      }

      public int GetFakeSpaceStartingPosition(int visualY)
      {
         var page = GetPageForPosition(visualY);
         return page == null ? 0 : page.EndDomainY;
      }

      public int GetFakeSpaceHeight(int visualY)
      {
         var page = GetPageForPosition(visualY);
         return page == null ? 0 : page.GetHeightOfFakeSpace();
      }

      public VMPage NextPage(CommonPageLayout actualPageSettings)
      {
         return CreatePage(actualPageSettings);
      }
      public VMPage NextPage(VMPage actualPage, CommonPageLayout actualPageSettings)
      {
         if (actualPage == null)
            return NextPage(actualPageSettings);
         else
         {
            var nextExistingPage = GetExistingNextPage(actualPage);

            if (nextExistingPage != null)
               return nextExistingPage;
            else
               return CreatePage(actualPageSettings);
         }
      }

      private VMPage GetExistingNextPage(VMPage actualPage)
      {
         var indexActualPage = Pages.IndexOf(actualPage);

         if (indexActualPage < 0)
            throw new System.Exception("The page does not belong to the list of pages."); //Should never occurs, just for testing purposes

         if ((Pages.Count - 1) >= (indexActualPage + 1))
            return Pages[indexActualPage + 1];
         else
            return null;
      }
      private VMPage CreatePage(CommonPageLayout actualPageSettings)
      {
         if (Pages.Count == 0)
            return CreateFirstPage(actualPageSettings);
         else
            return CreateNextPage(actualPageSettings);
      }
      private VMPage CreateNextPage(CommonPageLayout actualPageSettings)
      {
         VMPage previousPage = Pages.Last();
         VMPage newPage = AddPage(actualPageSettings);

         newPage.OffsetForComponentsOnThisPage = previousPage.OffsetForComponentsOnThisPage + VisualOffsetPerPage(actualPageSettings);
         newPage.StartVisualY = previousPage.StartVisualY + actualPageSettings.PageSize.Height + VMPage.SPACE_BETWEEN_PAGES;
         newPage.StartDomainY = previousPage.EndDomainY + 1;
         newPage.SetDefaultEndDomainYForThisStartDomainY();

         return newPage;
      }
      private VMPage CreateFirstPage(CommonPageLayout actualPageSettings)
      {
         ClearPages();
         var newPage = AddPage(actualPageSettings);

         newPage.StartVisualY = VisualOffsetFirstPage(actualPageSettings);
         newPage.OffsetForComponentsOnThisPage = newPage.StartVisualY;
         newPage.StartDomainY = 0;
         newPage.SetDefaultEndDomainYForThisStartDomainY();

         return newPage;
      }
      private VMPage AddPage(CommonPageLayout pageSettings)
      {
         VMReportBoundarySection vmReportHeader;
         VMReportBoundarySection vmReportFooter;

         if (pageSettings.HasHeader())
            vmReportHeader = _reportBoundarySectionFactory.CreateForHeader(pageSettings.Header.Height, GetNextPageNumber());
         else
            vmReportHeader = null;

         if (pageSettings.HasFooter())
            vmReportFooter = _reportBoundarySectionFactory.CreateForFooter(pageSettings.Footer.Height, GetNextPageNumber());
         else
            vmReportFooter = null;

         var newPage = new VMPage(pageSettings, _reportModeProperty, vmReportHeader, vmReportFooter);
         Pages.Add(newPage);

         TotalPagesHeight = (Pages.Count * (pageSettings.PageSize.Height + VMPage.SPACE_BETWEEN_PAGES)) + pageSettings.PageSize.Height;
         return newPage;
      }

      private int GetNextPageNumber()
      {
         return Pages.Count + 1;
      }

      public void ResetDomainInfoAffectedByDisabledSpace(DisabledSpaceData disabledSpace)
      {
         Pages.GetTail(disabledSpace.StartPage).ForEach(p => p.SetValuesAffectedByDisabledSpace(disabledSpace));
      }
   }
}