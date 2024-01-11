// <copyright file="HeaderFormFieldControllerFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Export.Fakes
{
   [ExcludeFromCodeCoverage]
   public class HeaderFormFieldControllerFake : IHeaderFormFieldController
   {
      public void UpdateSelectedHeaderFormField(IItemId<ReportHeaderFormField> reportHeaderFormFieldId, Guid selectedFieldId)
      {
      }

      public void UpdateSelectedHeaderFormFieldLabel(IItemId<ReportHeaderFormField> reportHeaderFormFieldId, string label)
      {
      }

      public void UpdateSelectedHeaderFormFieldLabelWidthPercentage(IItemId<ReportHeaderFormField> reportHeaderFormFieldId, double labelWidthPercentage)
      {
      }
   }
}