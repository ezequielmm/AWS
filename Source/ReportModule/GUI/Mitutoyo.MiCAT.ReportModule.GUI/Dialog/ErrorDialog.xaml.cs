// <copyright file="ErrorDialog.xaml.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Dialog
{
   [ExcludeFromCodeCoverage]
   public partial class ErrorDialog : Window, IDialog
    {
        public ErrorDialog()
        {
            InitializeComponent();
        }
    }
}