// <copyright file="FileDialogManager.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.IO;
using Microsoft.Win32;
using Mitutoyo.MiCAT.Core.StdLib.Extensions;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Dialog
{
   public class FileDialogManager : IDialogManager
   {
      public bool? ShowOpenFileDialog(string title, IFileDirectoryHolder dirHolder, string filters, int filterIndex, bool multiselect, ref string[] fileNames)
      {
         var dlg = new OpenFileDialog
         {
            Title = title,
            InitialDirectory = (dirHolder != null) ? dirHolder.FileDirectory : string.Empty,
            Filter = filters,
            FilterIndex = filterIndex,
            Multiselect = multiselect,
         };
         var result = dlg.ShowDialog();
         fileNames = dlg.FileNames;

         if (!fileNames.IsEmpty() && dirHolder != null)
            dirHolder.FileDirectory = Path.GetDirectoryName(fileNames[0]);

         return result;
      }
   }
}
