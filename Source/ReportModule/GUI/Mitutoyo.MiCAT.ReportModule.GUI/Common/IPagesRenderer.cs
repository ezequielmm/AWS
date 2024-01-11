// <copyright file="IPagesRenderer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElementList.ViewModels;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common
{
   public interface IPagesRenderer
   {
      IRenderedData RenderedData { get; }
      IVMReportElementList ElementList { get; }
      event EventHandler<LayoutRecalculationFinishEventArgs> LayoutRecalculationFinished;
   }
}
