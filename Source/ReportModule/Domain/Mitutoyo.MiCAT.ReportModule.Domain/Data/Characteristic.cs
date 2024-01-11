// <copyright file="Characteristic.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Data
{
   public class Characteristic
   {
      public Characteristic() : this(Guid.NewGuid(), string.Empty, null, null, null, null, null, null, null) { }

      public Characteristic(Guid id, string name, string characteristicType) : this(id, name, characteristicType, null, null, null, null, null, null) { }

      public Characteristic(Guid id, string name, string characteristicType, Feature feature, double? nominal, string details, double? upperTolerance, double? lowerTolerance, double? toleranceZone)
      {
         Id = id;
         Name = name;
         CharacteristicType = characteristicType;
         Feature = feature;
         Nominal = nominal;
         Detail = details;
         UpperTolerance = upperTolerance;
         LowerTolerance = lowerTolerance;
         ToleranceZone = toleranceZone;
      }

      public Guid Id { get; }
      public string Name { get; }
      public double? UpperTolerance { get; }
      public double? LowerTolerance { get; }
      public double? ToleranceZone { get; }
      public string CharacteristicType { get; }
      public string Detail { get; }
      public double? Nominal { get; }
      public Feature Feature { get; set; }

      public Characteristic WithName(string name)
      {
         return new Characteristic(Id, name, CharacteristicType, Feature, Nominal, Detail, UpperTolerance, LowerTolerance, ToleranceZone);
      }

      public Characteristic WithCharacteristicType(string characteristicType)
      {
         return new Characteristic(Id, Name, characteristicType, Feature, Nominal, Detail, UpperTolerance, LowerTolerance, ToleranceZone);
      }
   }
}