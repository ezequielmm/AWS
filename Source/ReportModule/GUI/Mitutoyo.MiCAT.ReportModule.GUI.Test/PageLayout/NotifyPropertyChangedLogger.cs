// <copyright file="NotifyPropertyChangedLogger.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.PageLayout
{
   [ExcludeFromCodeCoverage]
   public class NotifyPropertyChangedLogger
   {
      public List<string> ChangeLog { get; set; }

      public NotifyPropertyChangedLogger(INotifyPropertyChanged viewModel)
      {
         if (viewModel == null)
         {
            throw new ArgumentNullException("viewModel");
         }
         this.ChangeLog = new List<string>();
         viewModel.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
      }

      public void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         ChangeLog.Add(e.PropertyName);
      }
   }
}