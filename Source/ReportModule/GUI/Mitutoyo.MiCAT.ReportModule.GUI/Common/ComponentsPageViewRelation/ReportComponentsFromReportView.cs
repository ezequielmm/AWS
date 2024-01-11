// <copyright file="ReportComponentsFromReportView.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Containers.InteractiveControlContainer;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.ComponentsPageViewRelation
{
   public class ReportComponentsFromReportView : IReportComponentsFromReportView
   {
      public IEnumerable<InteractiveControlContainer> GetReportComponents(ReportView reportView)
      {
         return GetElements(reportView).Concat(GetPieces(reportView));
      }

      private IEnumerable<InteractiveControlContainer> GetElements(ReportView reportView)
      {
         VisualChildrensByType<InteractiveControlContainer> visualChildrensByTypeGetter = new VisualChildrensByType<InteractiveControlContainer>();

         return visualChildrensByTypeGetter.GetChildrensByType(reportView.Elements);
      }
      private IEnumerable<InteractiveControlContainer> GetPieces(ReportView reportView)
      {
         VisualChildrensByType<InteractiveControlContainer> visualChildrensByTypeGetter = new VisualChildrensByType<InteractiveControlContainer>();

         return visualChildrensByTypeGetter.GetChildrensByType(reportView.Pieces);
      }
   }
}
