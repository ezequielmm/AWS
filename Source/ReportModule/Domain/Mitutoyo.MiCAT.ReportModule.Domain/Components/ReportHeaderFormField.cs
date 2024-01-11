// <copyright file="ReportHeaderFormField.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ApplicationState;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Components
{
   public class ReportHeaderFormField : BaseStateEntity<ReportHeaderFormField>
   {
      public Guid SelectedFieldId { get; }
      public string CustomLabel { get; }
      public double LabelWidthPercentage { get; }

      public ReportHeaderFormField() : this(Guid.NewGuid(), Guid.Empty, null, 0) { }

      public ReportHeaderFormField(
         Id<ReportHeaderFormField> id,
         Guid selectedFieldId, string customLabel,
         double labelWidthPercentage)
         : base(id)
      {
         SelectedFieldId = selectedFieldId;
         CustomLabel = customLabel;
         LabelWidthPercentage = labelWidthPercentage;
      }

      public ReportHeaderFormField WithSelectedFieldId(Guid selectedFieldId)
      {
         return new ReportHeaderFormField(Id, selectedFieldId, null, LabelWidthPercentage);
      }

      public ReportHeaderFormField WithCustomLabel(string customLabel)
      {
         return new ReportHeaderFormField(Id, SelectedFieldId, customLabel, LabelWidthPercentage);
      }

      public ReportHeaderFormField WithLabelWidthPercentage(double labelWidthPercentage)
      {
         return new ReportHeaderFormField(Id, SelectedFieldId, CustomLabel, labelWidthPercentage);
      }
   }
}
