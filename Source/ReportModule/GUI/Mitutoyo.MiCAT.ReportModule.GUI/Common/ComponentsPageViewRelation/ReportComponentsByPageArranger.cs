// <copyright file="ReportComponentsByPageArranger.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Containers.InteractiveControlContainer;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.ComponentsPageViewRelation
{
   public class ReportComponentsByPageArranger : IReportComponentsByPageArranger
   {
      public IImmutableList<ReportComponentsByPage> ArrangeReportComponentsByPage(IEnumerable<PageView> pages, IEnumerable<InteractiveControlContainer> reportComponents)
      {
         var result = new List<ReportComponentsByPage>();

         foreach (var page in pages.OrderBy(p => (p.DataContext as VMPage)?.StartVisualY))
         {
            if (page.DataContext is VMPage vmPage)
            {
               var componentsForThisPage = new List<InteractiveControlContainer>();
               foreach (var component in reportComponents)
               {
                  if (IsComponentInsideThePage(component, vmPage))
                     componentsForThisPage.Add(component);
               }

               result.Add(new ReportComponentsByPage(page, componentsForThisPage.ToImmutableList()));
            }
         }

         return result.ToImmutableList();
      }

      private bool IsComponentInsideThePage(InteractiveControlContainer component, VMPage page)
      {
         if (component.DataContext is IVMVisualElement visualElement)
         {
            return (visualElement.VMPlacement.VisualY >= page.StartVisualY && visualElement.VMPlacement.VisualY <= page.EndVisualY && (visualElement.VMPlacement.VisualX + visualElement.VMPlacement.VisualWidth) > 0 && visualElement.VMPlacement.VisualX < page.Width);
         }

         return false;
      }
   }
}
