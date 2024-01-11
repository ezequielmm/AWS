// <copyright file="VMMarginSizeSettingsItem.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Strings;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.Filebar.MarginsSettings
{
   public class VMMarginSizeSettingsItem : VMBase
   {
      private bool _isChecked;
      private bool _isEnabled;
      private const string PREFIX_DISPLAY_NAME_MARGIN_SIZE = "Margin";
      private const string PREFIX_DESCRIPTION_MARGIN_SIZE = "MarginDescription";

      public VMMarginSizeSettingsItem(Margin margin)
      {
         Margin = margin;
         DisplayName = LocalizeDisplayName(margin.MarginKind);
         MarginDescription = LocalizeDescriptionName(margin.MarginKind);
      }

      public string DisplayName { get; }
      public string MarginDescription { get; }

      public Margin Margin { get; }

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

      private string LocalizeDisplayName(MarginKind marginKind) => StringFinder.FindLocalizedString(PREFIX_DISPLAY_NAME_MARGIN_SIZE + marginKind.ToString());
      private string LocalizeDescriptionName(MarginKind marginKind) => StringFinder.FindLocalizedString(PREFIX_DESCRIPTION_MARGIN_SIZE + marginKind.ToString());
   }
}
