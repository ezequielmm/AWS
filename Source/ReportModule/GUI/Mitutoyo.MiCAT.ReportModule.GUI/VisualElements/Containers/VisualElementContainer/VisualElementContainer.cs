// <copyright file="VisualElementContainer.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using System.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Containers.VisualElementContainer
{
   public class VisualElementContainer : ContentControl
   {
      protected override Size MeasureOverride(Size constraint)
      {
         if (Content is FrameworkElement element) element.Measure(constraint);

         return base.MeasureOverride(constraint);
      }
   }
}
