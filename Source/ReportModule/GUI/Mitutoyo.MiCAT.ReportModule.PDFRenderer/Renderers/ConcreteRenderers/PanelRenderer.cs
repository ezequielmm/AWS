// <copyright file="PanelRenderer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.ConcreteRenderers
{
   [ExcludeFromCodeCoverage]
   public class PanelRenderer : FrameworkElementRenderer
    {
        public PanelRenderer()
            : base(typeof(Panel))
        {
        }

        public override bool Render(UIElement element, FixedDocumentRenderContext context)
        {
            var panel = element as Panel;
            if (panel == null)
            {
                return false;
            }

            RectangleRenderer.DrawRectangle(context, panel.Background, null, 0, panel.ActualWidth, panel.ActualHeight, null);

            return base.Render(panel, context);
        }
    }
}
