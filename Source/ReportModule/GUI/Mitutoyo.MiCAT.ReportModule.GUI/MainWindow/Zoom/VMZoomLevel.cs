// <copyright file="VMZoomLevel.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.Zoom
{
   public class VMZoomLevel
   {
      private const string DISPLAY_FORMAT = "{0}%";

      public VMZoomLevel(double value)
      {
         Value = value;
      }

      public string Text => string.Format(DISPLAY_FORMAT, Value * 100);
      public double Value { get; set; }
   }
}
