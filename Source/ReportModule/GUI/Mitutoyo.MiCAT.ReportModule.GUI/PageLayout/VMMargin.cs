// <copyright file="VMMargin.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.GUI.Common;

namespace Mitutoyo.MiCAT.ReportModule.GUI.PageLayout
{
   public class VMMargin
   {
      public VMMargin(int position, int width, IReportModeProperty reportModeProperty)
      {
         Position = position;
         Width = width;
         ReportModeProperty = reportModeProperty;
      }
      public int Width { get; set; }
      public int Position { get; set; }
      public IReportModeProperty ReportModeProperty { get; set; }
   }
}
