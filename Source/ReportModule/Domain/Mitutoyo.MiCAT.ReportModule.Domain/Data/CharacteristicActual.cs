// <copyright file="CharacteristicActual.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Data
{
   public class CharacteristicActual
   {
      public CharacteristicActual(Guid characteristicId, string status, double? measured, double? deviation)
      {
         CharacteristicId = characteristicId;
         Status = status;
         Measured = measured;
         Deviation = deviation;
      }

      public Guid CharacteristicId { get; }
      public string Status { get; }
      public double? Measured { get; }
      public double? Deviation { get; }
   }
}
