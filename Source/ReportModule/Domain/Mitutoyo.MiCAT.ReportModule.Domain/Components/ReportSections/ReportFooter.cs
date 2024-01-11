// <copyright file="ReportFooter.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ApplicationState;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections
{
   public class ReportFooter : BaseStateEntity<ReportFooter>
   {
      public ReportFooter() : base(new Id<ReportFooter>(Guid.NewGuid()))
      {
      }

      private ReportFooter(Id<ReportFooter> id, bool isEnabled)
         : base(id)
      {
         IsEnabled = isEnabled;
      }

      public ReportFooter(bool isEnabled)
         : this(new Id<ReportFooter>(Guid.NewGuid()), isEnabled)
      {
      }

      public bool IsEnabled { get; }
   }
}
