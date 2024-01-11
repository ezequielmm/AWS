// <copyright file="PageLayoutCalculator.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElementList.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator
{
   public class PageLayoutCalculator : IPageLayoutCalculator
   {
      private IMultiPageElementManager _multiPageElementManager;

      public PageLayoutCalculator(IMultiPageElementManager multiPageElementManager)
      {
         _multiPageElementManager = multiPageElementManager;
      }

      public void CalculateComponentPositions(CommonPageLayout actualPageSettings, IVMReportElementList elementList, IRenderedData renderedData)
      {
         var ordererComponents = elementList.Elements.OfType<IVMReportComponent>().OrderBy(rc => rc.VMPlacement.DomainY).ThenByDescending(rc => rc.VMPlacement.DomainHeight);
         renderedData.Pages.ClearPages();
         renderedData.DisabledSpaces.Items.Clear();
         var actualPage = renderedData.Pages.NextPage(actualPageSettings);

         foreach (var component in ordererComponents)
         {
            if (component.VMPlacement is IVMReportBodyPlacement reportBodyPlacement)
            {
               reportBodyPlacement.PageVerticalOffset = actualPage.OffsetForComponentsOnThisPage;

               reportBodyPlacement.SetExtraOffset(renderedData.DisabledSpaces.Items.Where(ds => ds.IsAffectedByDisabledSpace(component.VMPlacement.DomainY)).Sum(pe => pe.UsableSpaceTaken));

               while (!actualPage.YVisualPositionIsOnThisPage(component.VMPlacement.VisualY))
               {
                  actualPage = renderedData.Pages.NextPage(actualPage, actualPageSettings);
                  reportBodyPlacement.PageVerticalOffset = actualPage.OffsetForComponentsOnThisPage;
               }

               if (actualPage.ReportElementWouldFitOnThisPage(component))
               {
                  if (!actualPage.PlacementWouldFitOnThisPageOnItsPosition(reportBodyPlacement))
                  {
                     var spaceLeftOnPreviousPage = actualPage.SpaceLeftOnThisPage(reportBodyPlacement.VisualY);

                     actualPage.SetPageBreakInfoByComponent(reportBodyPlacement);

                     actualPage = renderedData.Pages.NextPage(actualPage, actualPageSettings);
                     actualPage.OffsetForComponentsOnThisPage += spaceLeftOnPreviousPage + 1;

                     reportBodyPlacement.PageVerticalOffset = actualPage.OffsetForComponentsOnThisPage;
                  }
               }

               if (component is IVMMultiPageSplittableElement splitableComponent)
               {
                  _multiPageElementManager.RenderMultiPageElement(splitableComponent, actualPageSettings, elementList, renderedData);
               }

               if (component is IDisabledSpaceGenerator disabledSpaceGenerator)
               {
                  var disabledSpace = disabledSpaceGenerator.GetDisabledSpace();
                  renderedData.DisabledSpaces.Items.Add(disabledSpace);
                  renderedData.Pages.ResetDomainInfoAffectedByDisabledSpace(disabledSpace);
               }
            }
         }
      }
   }
}
