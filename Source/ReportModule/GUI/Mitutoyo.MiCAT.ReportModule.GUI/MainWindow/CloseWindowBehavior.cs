// <copyright file="CloseWindowBehavior.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Interactivity;

namespace Mitutoyo.MiCAT.ReportModule.GUI
{
   [ExcludeFromCodeCoverage]
   public class CloseWindowBehavior : Behavior<Window>
   {
      public bool CloseTrigger
      {
         get { return (bool)GetValue(CloseTriggerProperty); }
         set { SetValue(CloseTriggerProperty, value); }
      }

      public static readonly DependencyProperty CloseTriggerProperty =
         DependencyProperty.Register("CloseTrigger", typeof(bool), typeof(CloseWindowBehavior), new PropertyMetadata(false, OnCloseTriggerChanged));

      private static void OnCloseTriggerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var behavior = d as CloseWindowBehavior;

         behavior?.OnCloseTriggerChanged();
      }

      private void OnCloseTriggerChanged()
      {
         if (this.CloseTrigger)
         {
            this.AssociatedObject.Close();
         }
      }
   }
}
