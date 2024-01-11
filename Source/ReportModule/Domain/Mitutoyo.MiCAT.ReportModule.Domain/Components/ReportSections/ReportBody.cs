// <copyright file="ReportBody.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ApplicationState;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections
{
   public class ReportBody : BaseStateEntity<ReportBody>
   {
      public ReportBody()
         : base(new Id<ReportBody>(Guid.NewGuid()))
      {
      }

      public ReportBody(bool isEnabled)
         : this(new Id<ReportBody>(Guid.NewGuid()), isEnabled)
      {
      }

      private ReportBody(Id<ReportBody> id, bool isEnabled)
        : base(id)
      {
         IsEnabled = isEnabled;
      }

      public bool IsEnabled { get; }
   }
}
