// <copyright file="ReportHeaderFormFieldSO.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.ExtraStateItems.ReportHeaderForm
{
   public class ReportHeaderFormFieldSO
   {
      public Guid Id { get; set; }
      public Guid SelectedFieldId { get; set; }
      public string CustomLabel { get; set; }
      public double LabelWidthPercentage { get; set; }
   }
}
