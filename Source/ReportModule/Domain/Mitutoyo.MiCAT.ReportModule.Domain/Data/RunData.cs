// <copyright file="RunData.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Immutable;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Data
{
   public class RunData
   {
      public RunData (DateTime? timeStamp, Guid planId, int planVersion, ImmutableList<EvaluatedCharacteristic> characteristicList, ImmutableList<DynamicPropertyValue> dynamicPropertyValues, ImmutableList<Capture3D> captures3D)
      {
         TimeStamp = timeStamp;
         PlanId = planId;
         PlanVersion = planVersion;
         CharacteristicList = characteristicList;
         DynamicPropertyValues = dynamicPropertyValues;
         Captures3D = captures3D;
      }

      public DateTime? TimeStamp { get; }
      public Guid PlanId { get; }
      public int PlanVersion { get; }
      public ImmutableList<EvaluatedCharacteristic> CharacteristicList { get; }
      public ImmutableList<DynamicPropertyValue> DynamicPropertyValues { get; }
      public ImmutableList<Capture3D> Captures3D { get; }
   }
}
