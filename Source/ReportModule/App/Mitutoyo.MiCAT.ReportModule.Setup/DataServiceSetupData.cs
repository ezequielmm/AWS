// <copyright file="DataServiceSetupData.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;

namespace Mitutoyo.MiCAT.ReportModule.Setup
{
   public class DataServiceSetupData
   {
      public IEnumerable<DynamicPropertyDescriptor> DynamicProperties { get; set; }
      public IEnumerable<PlanDescriptor> Plans { get; set; }
      public IEnumerable<string> CharacteristicsTypes { get; set; }
      public IEnumerable<string> AllCharacteristicDetails { get; set; }
   }
}
