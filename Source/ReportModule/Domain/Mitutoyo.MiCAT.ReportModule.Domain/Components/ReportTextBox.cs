// <copyright file="ReportTextBox.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ApplicationState;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Components
{
   public class ReportTextBox : ReportComponentBase<ReportTextBox>, IReportComponent
   {
      private const int DEFAULT_WIDTH = 100;
      private const int DEFAULT_HEIGHT = 25;

      public ReportTextBox(IItemId reportSectionId, int x, int y)
         : this(new ReportComponentPlacement(reportSectionId, x, y, DEFAULT_WIDTH, DEFAULT_HEIGHT))
      { }
      public ReportTextBox(ReportComponentPlacement placement)
         : this(placement, string.Empty)
      { }
      public ReportTextBox(ReportComponentPlacement placement, string text)
         : this(Guid.NewGuid(), placement, text)
      { }

      private ReportTextBox(Id<ReportTextBox> id, ReportComponentPlacement placement, string text)
         : base(id, placement)
      {
         Text = text;
      }

      public string Text { get; }

      public ReportTextBox WithText(string text)
      {
         return new ReportTextBox(Id, Placement, text);
      }

      public override ReportTextBox WithPosition(int x, int y)
      {
         return new ReportTextBox(Id, Placement.WithPosition(x, y), Text);
      }

      public override ReportTextBox WithSize(int widht, int height)
      {
         return new ReportTextBox(Id, Placement.WithSize(widht, height), Text);
      }
   }
}
