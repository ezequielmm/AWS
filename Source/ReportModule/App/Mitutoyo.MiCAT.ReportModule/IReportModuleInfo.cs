// <copyright file="IReportModuleInfo.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ShellModule;

namespace Mitutoyo.MiCAT.ReportModule
{
   using System.Collections.Immutable;
   using ApplicationState;
   public interface IReportModuleInfo
   {
      Id ModuleId { get; }

      ImmutableList<Id> ChildIds { get; }

      ISnapShot AddChildItems(NavigationModuleInfo moduleInfo, ISnapShot snapShot, NavigationModuleChildInfo childInfo);

      NavigationModuleChildInfo CreateChildItem();

      void RemoveChildItem(Id id);

      bool ChildIdExists(Id reportId);
   }
}
