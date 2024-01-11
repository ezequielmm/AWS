// <copyright file="ReportHeaderFormRowSO.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.ExtraStateItems.ReportHeaderForm
{
   public class ReportHeaderFormRowSO
   {
      public Guid Id { get; set; }

      [XmlArray("FieldIds")]
      [XmlArrayItem("FieldId")]
      public List<Guid> FieldIds { get; set; }
   }
}
