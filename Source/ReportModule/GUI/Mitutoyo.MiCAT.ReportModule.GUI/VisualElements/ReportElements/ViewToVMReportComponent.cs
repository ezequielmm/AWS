// <copyright file="ViewToVMReportComponent.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements
{
   public class ViewToVMReportComponent : IViewToVMReportComponent
   {
      public IVMReportComponent GetVMReportElementFromOriginalSource(RoutedEventArgs routedEventArgs)
      {
         if (routedEventArgs?.OriginalSource is FrameworkElement originalSourceElement)
            return GetVMReportElement(originalSourceElement);
         else
            return null;
      }

      public IVMReportComponent GetVMReportElement(FrameworkElement viewElement)
      {
         if (viewElement == null)
         {
            return null;
         }
         else
         {
            if (viewElement.DataContext is IVMReportComponent vmReportComponent)
            {
               return vmReportComponent;
            }
            else
            {
               return GetVMReportElementFromParentElement(viewElement);
            }
         }
      }
      private IVMReportComponent GetVMReportElementFromParentElement(FrameworkElement viewElement)
      {
         if (viewElement?.Parent is FrameworkElement parentElement)
         {
            return GetVMReportElement(parentElement);
         }
         else
         {
            return null;
         }
      }

      public bool IsViewOfReportElement(FrameworkElement viewElement)
      {
         return (GetVMReportElement(viewElement) != null);
      }

      public bool IsOriginalSourceViewOfReportElement(RoutedEventArgs routedEventArgs)
      {
         return (GetVMReportElementFromOriginalSource(routedEventArgs) != null);
      }
   }
}
