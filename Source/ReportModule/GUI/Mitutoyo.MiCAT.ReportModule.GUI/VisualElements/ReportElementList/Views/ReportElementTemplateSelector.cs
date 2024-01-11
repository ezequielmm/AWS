// <copyright file="ReportElementTemplateSelector.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using System.Windows.Controls;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Image.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.TesselationView.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.TextBox.ViewModels;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElementList.Views
{
   public class ReportElementTemplateSelector : DataTemplateSelector
   {
      public DataTemplate TextBoxTemplate{ get; set; }

      public DataTemplate TableViewTemplate { get; set; }

      public DataTemplate TessellationViewTemplate { get; set; }

      public DataTemplate ImageTemplate { get; set; }

      public DataTemplate HeaderFormTemplate { get; set; }

      public DataTemplate TableViewPieceTemplate { get; set; }

      public override DataTemplate SelectTemplate(object item, DependencyObject container)
      {
         switch (item)
         {
            case VMTextBox _:
               return TextBoxTemplate;

            case VMTable _:
               return TableViewTemplate;

            case VMTessellationView _:
               return TessellationViewTemplate;

            case VMImage _:
               return ImageTemplate;

            case IVMHeaderForm _:
               return HeaderFormTemplate;

            case VMTablePiece _:
               return TableViewPieceTemplate;

            default:
               return base.SelectTemplate(item, container);
         }
      }
   }
}
