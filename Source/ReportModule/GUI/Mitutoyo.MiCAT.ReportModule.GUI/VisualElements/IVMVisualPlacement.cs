// <copyright file="IVMVisualPlacement.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements
{
   public interface IVMVisualPlacement
   {
      int VisualX { get; }
      int VisualY { get; }
      int VisualWidth { get; }
      int VisualHeight { get; }

      bool AutoAdjustVisualHeight { get; }
      bool AutoAdjustVisualWidth { get; }

      void SetVisualPosition(int visualX, int visualY);
      void SetVisualSize(int visualWidth, int visualHeight);
   }
}
