// <copyright file="PageSizeMenuItem.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>
using System.Drawing.Printing;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;

namespace Mitutoyo.MiCAT.ReportModule.GUI.PageLayout
{
   public class PageSizeMenuItem : VMBase
   {
      private string _paperName;
      private PaperKind _paperKind;
      private PaperKind _currentPaperKind;
      private bool _isChecked;

      public string PaperName
      {
         get { return _paperName; }
         set
         {
            _paperName = value;
            RaisePropertyChanged();
         }
      }
      public PaperKind PaperKind
      {
         get { return _paperKind; }
         set
         {
            _paperKind = value;
            RaisePropertyChanged();
         }
      }
      public PaperKind CurrentPaperKind
      {
         get { return _currentPaperKind; }
         set
         {
            _currentPaperKind = value;
            RaisePropertyChanged();
         }
      }
      public bool IsChecked
      {
         get { return _isChecked; }
         set
         {
            _isChecked = value;
            RaisePropertyChanged();
         }
      }
   }
}
