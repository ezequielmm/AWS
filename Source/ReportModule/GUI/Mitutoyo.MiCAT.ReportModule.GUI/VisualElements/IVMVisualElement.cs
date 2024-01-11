// <copyright file="IVMVisualElement.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements
{
   public interface IVMVisualElement
   {
      IVMVisualPlacement VMPlacement { get; }
      DisplayMode DisplayMode { get; }
      RenderMode RenderMode { get; }
      void SetDisplayMode(DisplayMode newDisplayMode);
      void SetRenderMode(RenderMode newRenderMode);
   }
   public enum DisplayMode
   {
      Normal,
      Placement,
   }

   public enum RenderMode
   {
      ViewMode,
      EditMode,
   }
}
