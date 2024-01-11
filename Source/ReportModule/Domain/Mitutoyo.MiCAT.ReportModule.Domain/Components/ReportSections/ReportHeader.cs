// <copyright file="ReportHeader.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ApplicationState;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections
{
   public class ReportHeader : BaseStateEntity<ReportHeader>
   {
      public ReportHeader() : base(new Id<ReportHeader>(Guid.NewGuid()))
      {
      }

      private ReportHeader(Id<ReportHeader> id, bool isEnabled)
         : base(id)
      {
         IsEnabled = isEnabled;
      }

      public ReportHeader(bool isEnabled)
         : this(new Id<ReportHeader>(Guid.NewGuid()), isEnabled)
      {
      }

      public bool IsEnabled { get; }
   }
}
