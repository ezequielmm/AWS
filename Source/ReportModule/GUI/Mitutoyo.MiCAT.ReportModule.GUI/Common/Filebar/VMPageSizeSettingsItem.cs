// <copyright file="VMPageSizeSettingsItem.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Drawing.Printing;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Strings;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar
{
   public class VMPageSizeSettingsItem : VMBase
   {
      private bool _isChecked;
      private bool _isEnabled;
      private const string PREFIX_DISPLAY_NAME_PAGE_SIZE = "PageSize";

      public VMPageSizeSettingsItem(PageSizeInfo pageSizeInfo)
      {
         PageSizeInfo = pageSizeInfo;
         DisplayName = LocalizeDisplayName(pageSizeInfo.PaperKind);
      }

      public string DisplayName { get; }

      public PageSizeInfo PageSizeInfo { get; }

      public bool IsChecked
      {
         get { return _isChecked; }
         set
         {
            _isChecked = value;
            RaisePropertyChanged();
         }
      }

      public bool IsEnabled
      {
         get { return _isEnabled; }
         set
         {
            _isEnabled = value;
            RaisePropertyChanged();
         }
      }

      private string LocalizeDisplayName(PaperKind paperKind) => StringFinder.FindLocalizedString(PREFIX_DISPLAY_NAME_PAGE_SIZE + paperKind.ToString());
   }
}
