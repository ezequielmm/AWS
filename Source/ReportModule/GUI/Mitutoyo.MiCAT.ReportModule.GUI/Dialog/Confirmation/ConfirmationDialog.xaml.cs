// <copyright file="ConfirmationDialog.xaml.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Dialog.Confirmation
{
   /// <summary>
   /// Interaction logic for ConfirmationDialog.xaml
   /// </summary>
   [ExcludeFromCodeCoverage]
   public partial class ConfirmationDialog : Window, IDialog
   {
      public ConfirmationDialog()
      {
         InitializeComponent();
      }
   }
}
