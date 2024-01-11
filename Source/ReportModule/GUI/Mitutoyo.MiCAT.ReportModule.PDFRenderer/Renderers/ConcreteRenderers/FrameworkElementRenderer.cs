// <copyright file="FrameworkElementRenderer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.ConcreteRenderers
{
   [ExcludeFromCodeCoverage]
   public class FrameworkElementRenderer : UIElementRendererBase
    {
        private Type[] types;

        public FrameworkElementRenderer(params Type[] types)
        {
            this.types = types;
        }

       public override bool Render(UIElement element, FixedDocumentRenderContext context)
        {
            var frameworkElement = element as FrameworkElement;
            if (frameworkElement == null)
            {
                return false;
            }

           var elementType = frameworkElement.GetType();
           var hasMatchingType = this.types.Any(type => elementType == type || elementType.IsSubclassOf(type));

           if (!hasMatchingType)
            {
                return false;
            }

            var uiElements = new List<UIElement>();
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(frameworkElement); i++)
            {
                var child = VisualTreeHelper.GetChild(frameworkElement, i) as UIElement;
                uiElements.Add(child);
            }

            uiElements = uiElements.OrderBy(el => Canvas.GetZIndex(el)).ToList();

            foreach (var child in uiElements)
            {
                context.Facade.Render(child, context);
            }

            return true;
        }
    }
}
