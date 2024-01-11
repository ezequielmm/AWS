// <copyright file="IVMReportBodyPlacement.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements
{
   public interface IVMReportBodyPlacement : IVMReportComponentPlacement
   {
      int PageVerticalOffset { get; set; }
      void SetExtraOffset(int extraOffset);

      event EventHandler<ReportComponentMovedEventArgs> ReportComponentMoved;
      event EventHandler<ReportComponentLayoutChangedEventArgs> ReportComponentLayoutChanged;

      VMPage GetPage();
      VMPage GetPageForPlacement(IVMVisualPlacement vmPlacement);
      void RaiseReportComponentLayoutChanged();
   }
}
