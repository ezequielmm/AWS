// <copyright file="SelectionAdorner.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Containers.InteractiveControlContainer
{
   [ExcludeFromCodeCoverage]
   public class SelectionAdorner : Adorner
   {
      public SelectionAdorner(InteractiveControlContainer adornedElement) :
         base(adornedElement)
      {
         VisualChildrens = new VisualCollection(this);
         InitializeAdorner();
      }

      private VisualCollection VisualChildrens { get; }
      private InteractiveControlContainer ReportContentControl => (InteractiveControlContainer)AdornedElement;

      protected override int VisualChildrenCount => VisualChildrens.Count;
      protected override Visual GetVisualChild(int index) => VisualChildrens[index];

      protected override Size ArrangeOverride(Size finalSize)
      {
         VisualChildrens
            .Cast<SelectionMarker>()
            .ToList()
            .ForEach(x => x.ArrangeIn(ReportContentControl));

         return base.ArrangeOverride(finalSize);
      }

      protected override Size MeasureOverride(Size constraint)
      {
         VisualChildrens
            .Cast<SelectionMarker>()
            .ToList()
            .ForEach(x => x.ArrangeIn(ReportContentControl));

         return base.MeasureOverride(constraint);
      }

      private void InitializeAdorner()
      {
         Enum.GetValues(typeof(MarkerType))
            .Cast<MarkerType>()
            .ToList()
            .ForEach(x => AddMarker(x));
      }

      private void AddMarker(MarkerType type)
      {
         var marker = new SelectionMarker(ReportContentControl, type)
         {
            Width = 10,
            Height = 10,
            DataContext = ReportContentControl.DataContext, // REFACTOR: This is not needed by the selection marker, it is needed because of the unselection logic.
         };

         VisualChildrens.Add(marker);
      }
   }
}
