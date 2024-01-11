// <copyright file="Renderer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.ConcreteRenderers;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Editing;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers
{
   public class Renderer: IRenderer
   {
      private readonly List<UIElementRendererBase> renderers;

      public List<UIElementRendererBase> Renderers => renderers;

      public Renderer()
      {
         renderers = new List<UIElementRendererBase>();
         Initialize();
      }

      private void Initialize()
      {
         AddRenderer(new RadRichTextBoxRenderer());
         AddRenderer(new PanelRenderer());
         AddRenderer(new TextBoxRenderer());
         AddRenderer(new TextBlockRenderer());
         AddRenderer(new BorderRenderer());
         AddRenderer(new RectangleRenderer());
         AddRenderer(new EllipseRenderer());
         AddRenderer(new LineRenderer());
         AddRenderer(new ImageRenderer());
         AddRenderer(new ShapeRenderer());
         AddRenderer(new FrameworkElementRenderer(
            typeof(ContentPresenter),
            typeof(Control),
            typeof(ContentControl),
            typeof(ItemsPresenter),
            typeof(System.Windows.Documents.AdornerLayer)));
      }

      private void AddRenderer(UIElementRendererBase renderer)
      {
         Renderers.Add(renderer);
      }

      public bool Render(UIElement element, UIElement positionRelativeElement, FixedContentEditor drawingSurface)
      {
         var context = new FixedDocumentRenderContext(drawingSurface, this);
         return Render(element, positionRelativeElement, context);
      }

      public bool Render(UIElement element, FixedDocumentRenderContext context)
      {
         return Render(element, null, context);
      }
      public bool Render(UIElement element, UIElement positionRelativeElement, FixedDocumentRenderContext context)
      {
         if (element == null || element.Visibility != Visibility.Visible || element.Opacity == 0)
         {
            return false;
         }

         var compositeSave = new CompositeDisposableObject();
         compositeSave.Add(UIElementRendererBase.SaveMatrixPosition(context.DrawingSurface, element as FrameworkElement, positionRelativeElement));
         compositeSave.Add(UIElementRendererBase.SaveClip(context.DrawingSurface, element));
         compositeSave.Add(UIElementRendererBase.SaveOpacity(context, context.Opacity * element.Opacity));

         using (compositeSave)
         {
            foreach (UIElementRendererBase renderer in this.Renderers)
            {
               var success = renderer.Render(element, context);
               if (success)
               {
                  return true;
               }
            }
         }

         System.Diagnostics.Debug.WriteLine(string.Empty + element + " could not be exported correctly.");
         return false;
      }
   }
}
