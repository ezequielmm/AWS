// <copyright file="UnsavedChangesServiceFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.Applications.Services.Core;
using Mitutoyo.MiCAT.ApplicationState;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Export.Fakes
{
   [ExcludeFromCodeCoverage]
   public class UnsavedChangesServiceFake : IUnsavedChangesService
   {
      public bool IsServiceRunning { get => false; set { } }

      public bool HaveUnsavedChanges(ISnapShot currentSnapShot)
      {
         return false;
      }

      public void SaveSnapShot(ISnapShot lastSavedSnapShot)
      {
      }
   }
}
