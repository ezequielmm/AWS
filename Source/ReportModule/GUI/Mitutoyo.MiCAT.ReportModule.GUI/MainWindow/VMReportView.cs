// <copyright file="VMReportView.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using Mitutoyo.MiCAT.ReportModule.GUI.ContextMenu;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;

namespace Mitutoyo.MiCAT.ReportModule.GUI.MainWindow
{
   public class VMReportView : VMBase
   {
      public VMReportView(IPagesRenderer pagesRenderer, IReportModeProperty reportModeProperty,
         IMainContextMenu mainContextMenu)
      {
         PagesRenderer = pagesRenderer;
         ReportModeProperty = reportModeProperty;
         ContextMenu = mainContextMenu;

         OnContextMenuOpening = new Utilities.RelayCommand<ContextMenuEventArgs>(OnContextMenuOpen);
      }
      public IPagesRenderer PagesRenderer { get; }
      public IReportModeProperty ReportModeProperty { get; }
      public ICommand OnContextMenuOpening { get; }

      private void OnContextMenuOpen(ContextMenuEventArgs parameter)
      {
         SetComponentPosition(parameter);
      }

      private void SetComponentPosition(ContextMenuEventArgs parameter)
      {
         if (parameter.OriginalSource is FrameworkElement frameworkElement)
         {
            if (frameworkElement.DataContext is VMPage page)
            {
               var yRelativeToPage = (int)Math.Round(parameter.CursorTop, 0);
               var visualY = page.GetVisuaYFromYRelativeToThisPage(yRelativeToPage);

               if (!PagesRenderer.RenderedData.IsInDisabledSpace(visualY))
               {
                  var x = (int)Math.Round(parameter.CursorLeft, 0);

                  FocusManager.SetFocusedElement(FocusManager.GetFocusScope(frameworkElement), null);
                  ContextMenu.SetPagePosition(x, visualY);
               }
               else
                  parameter.Handled = true;
            }
            else
               parameter.Handled = true;
         }
      }

      public IMainContextMenu ContextMenu { get; }
   }
}
