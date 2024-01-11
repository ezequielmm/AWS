// <copyright file="RadContextMenuXamlHolder.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.ComponentModel;
using System.Windows.Markup;
using Telerik.Windows.Controls;

namespace Mitutoyo.MiCAT.ReportModule.GUI.ContextMenu
{
   [ContentProperty("ContextMenu")]
   public class RadContextMenuXamlHolder : INotifyPropertyChanged
   {
      private RadContextMenu contextMenu;
      public event PropertyChangedEventHandler PropertyChanged;

      public RadContextMenu ContextMenu
      {
         get
         {
            return this.contextMenu;
         }
         set
         {
            if (this.contextMenu != value)
            {
               this.contextMenu = value;
               this.RaisePropertyChanged("ContextMenu");
            }
         }
      }

      private void RaisePropertyChanged(string propertyName)
      {
         if (this.PropertyChanged != null)
         {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
         }
      }
   }
}
