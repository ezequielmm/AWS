// <copyright file="ReportTableViewSO.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Xml.Serialization;
using Mitutoyo.MiCAT.ReportModule.Persistence.Components.TableView;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Components
{
   public class ReportTableViewSO : ReportComponentSO
   {
      public double HeaderHeight { get; set; }
      public double RowHeight { get; set; }
      public double TableWidth { get; set; }

      [XmlArray("Columns")]
      [XmlArrayItem("Column", typeof(ColumnSO))]
      public List<ColumnSO> Columns { get; set; }

      [XmlArray("Sorting")]
      [XmlArrayItem("Column", typeof(SortingColumnSO))]
      public List<SortingColumnSO> Sorting { get; set; }

      [XmlArray("Filters")]
      [XmlArrayItem("Column", typeof(FilterColumnSO))]
      public List<FilterColumnSO> Filters { get; set; }

      [XmlArray("GroupBy")]
      [XmlArrayItem("Column", typeof(string))]
      public List<string> GroupBy { get; set; }
   }
}
