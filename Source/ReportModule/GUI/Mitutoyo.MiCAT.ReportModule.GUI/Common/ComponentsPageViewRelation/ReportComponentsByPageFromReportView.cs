// <copyright file="ReportComponentsByPageFromReportView.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Immutable;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.ComponentsPageViewRelation
{
   public class ReportComponentsByPageFromReportView : IReportComponentsByPageFromReportView
   {
      private IReportComponentsByPageArranger _reportComponentsByPageArranger;
      private IReportComponentsFromReportView _reportComponentsGetter;
      private IPageViewsFromReportView _pageViewsGetter;

      public ReportComponentsByPageFromReportView(
         IReportComponentsByPageArranger reportComponentsByPageArranger,
         IReportComponentsFromReportView reportComponentsGetter,
         IPageViewsFromReportView pageViewsGetter
         )
      {
         _reportComponentsByPageArranger = reportComponentsByPageArranger;
         _reportComponentsGetter = reportComponentsGetter;
         _pageViewsGetter = pageViewsGetter;
      }
      public IImmutableList<ReportComponentsByPage> GetReportComponentsByPage(ReportView reportView)
      {
         return _reportComponentsByPageArranger.ArrangeReportComponentsByPage(
               _pageViewsGetter.GetPageViews(reportView),
               _reportComponentsGetter.GetReportComponents(reportView)
            );
      }
   }
}
