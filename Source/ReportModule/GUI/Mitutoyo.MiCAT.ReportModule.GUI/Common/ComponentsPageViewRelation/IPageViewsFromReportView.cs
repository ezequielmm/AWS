// <copyright file="IPageViewsFromReportView.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.ComponentsPageViewRelation
{
   public interface IPageViewsFromReportView
   {
      IEnumerable<PageView> GetPageViews(ReportView reportView);
   }
}
