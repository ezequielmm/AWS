// <copyright file="ReportTemplateSO.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Xml.Serialization;
using Mitutoyo.MiCAT.ReportModule.Persistence.CADElements;
using Mitutoyo.MiCAT.ReportModule.Persistence.ExtraStateItems.ReportHeaderForm;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Components
{
   [XmlRoot("ReportTemplate")]
   public class ReportTemplateSO
   {
      public CommonPageLayoutSO CommonPageLayout { get; set; }

      [XmlArray("CadLayouts")]
      [XmlArrayItem("CadLayout", typeof(CADLayoutSO))]
      public List<CADLayoutSO> CadLayouts { get; set; }

      [XmlArray("ReportComponents")]
      [XmlArrayItem("TexBox", typeof(ReportTextBoxSO))]
      [XmlArrayItem("Table", typeof(ReportTableViewSO))]
      [XmlArrayItem("Image", typeof(ReportImageSO))]
      [XmlArrayItem("TessellationView", typeof(ReportTessellationViewSO))]
      [XmlArrayItem("HeaderForm", typeof(ReportHeaderFormSO))]
      public List<ReportComponentSO> ReportComponents { get; set; }

      [XmlArray("ReportComponentDataItems")]
      [XmlArrayItem("HeaderFormRow", typeof(ReportHeaderFormRowSO))]
      [XmlArrayItem("HeaderFormField", typeof(ReportHeaderFormFieldSO))]
      public List<object> ReportComponentDataItems { get; set; }
   }
}
