// <copyright file="VMVisualElement.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.GUI.Common;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements
{
   public class VMVisualElement : VMBase, IVMVisualElement
   {
      private DisplayMode _displayMode;
      private RenderMode _renderMode;

      public VMVisualElement(IVMVisualPlacement vmVisualPlacement)
      {
         VMPlacement = vmVisualPlacement;
      }
      public IVMVisualPlacement VMPlacement { get; }

      public DisplayMode DisplayMode
      {
         get => _displayMode;
         private set
         {
            _displayMode = value;
            RaisePropertyChanged();
         }
      }

      public RenderMode RenderMode
      {
         get => _renderMode;
         private set
         {
            _renderMode = value;
            RaisePropertyChanged();
         }
      }

      public virtual void SetDisplayMode(DisplayMode newDisplayMode)
      {
         if (DisplayMode != newDisplayMode)
            DisplayMode = newDisplayMode;
      }
      public virtual void SetRenderMode(RenderMode newRenderMode)
      {
         if (RenderMode != newRenderMode)
            RenderMode = newRenderMode;
      }
   }
}
