// <copyright file="ReportTemplateContentSerializerSelector.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template.FormatSerializers;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Template
{
   public class ReportTemplateContentSerializerSelector : IReportTemplateContentSerializerSelector
   {
      private List<IReportTemplateContentSerializer> serializers = new List<IReportTemplateContentSerializer>();

      public ReportTemplateContentSerializerSelector()
      {
         serializers.Add(new XMLReportTemplateContentSerializer());
      }

      public IReportTemplateContentSerializer Select(VersionIdentifierSO versionIdentifier)
      {
         string version = versionIdentifier.Version;

         var serializer = serializers.SingleOrDefault(s => s.SupportsVersion(version));
         if (serializer != null) return serializer;

         throw new ResultException("Incompatible Template Version", "IncompatibleTemplateError");
      }
   }
}
