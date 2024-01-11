// <copyright file="ProcessLauncher.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics;

namespace Mitutoyo.MiCAT.ReportModuleApp.Utilities
{
   public class ProcessLauncher : IProcessLauncher
   {
      public void LaunchProcess(string fileName)
      {
         using (var p = new Process())
         {
            p.StartInfo = new ProcessStartInfo(fileName)
            {
               UseShellExecute = true
            };
            p.Start();
         }
      }
   }
}
