// <copyright file="ScrollviewerEventToCommand.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Interop;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Utilities
{
   public class ScrollviewerEventToCommand : TriggerAction<ScrollViewer>
   {
      public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ScrollviewerEventToCommand), new PropertyMetadata(null, (s, e) => { }));

      public ICommand Command
      {
         get
         {
            return (ICommand)GetValue(CommandProperty);
         }

         set
         {
            SetValue(CommandProperty, value);
         }
      }

      private bool AssociatedElementIsDisabled()
      {
         var scrollViewer = AssociatedObject as ScrollViewer;
         return AssociatedObject == null || (scrollViewer != null && !scrollViewer.IsEnabled);
      }

      protected override void Invoke(object parameter)
      {
         if (AssociatedElementIsDisabled())
         {
            return;
         }

         if (InvokeByMouseEvent(parameter))
         {
            return;
         }
         if (InvokeByKeyEvent(parameter))
         {
            return;
         }
      }

      private bool InvokeByMouseEvent(object parameter)
      {
         var mouseEventArgs = parameter as MouseWheelEventArgs;
         if (mouseEventArgs == null)
         {
            return false;
         }

         if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
         {
            mouseEventArgs.Handled = true;

            if (mouseEventArgs.Delta > 0)
            {
               Invoke(Operation.ZoomIn);
            }
            if (mouseEventArgs.Delta < 0)
            {
               Invoke(Operation.ZoomOut);
            }
         }
         // Removed lines to fix scrolling issue,
         // scroll gets cut always without taking care if the mouse is over another scrollviewer

         return true;
      }

      private bool InvokeByKeyEvent(object parameter)
      {
         var keyEventArgs = parameter as KeyboardEventArgs;
         if (keyEventArgs == null)
         {
            return false;
         }

         if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
         {
            if (Keyboard.IsKeyDown(Key.Add))
            {
               Invoke(Operation.ZoomIn);
            }
            if (Keyboard.IsKeyDown(Key.Subtract))
            {
               Invoke(Operation.ZoomOut);
            }
         }

         return true;
      }

      private void Invoke(Operation operation)
      {
         var args = new ZoomInOutCommandArgs();
         args.ZoomCommand = operation;

         var command = Command;
         if (command != null && command.CanExecute(args) && !IsPopUpOpen())
         {
            command.Execute(args);
         }
      }

      private bool IsPopUpOpen()
      {
         return PresentationSource
            .CurrentSources
            .OfType<HwndSource>()
            .Select(h => h.RootVisual)
            .OfType<FrameworkElement>()
            .Select(f => f.Parent)
            .OfType<Popup>()
            .Any(p => p.IsOpen);
      }
   }
}
