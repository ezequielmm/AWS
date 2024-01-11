// <copyright file="VMEvaluatedCharacteristic.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModule.GUI.Strings;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels
{
   public class VMEvaluatedCharacteristic
   {
      public VMEvaluatedCharacteristic(EvaluatedCharacteristic evaluatedCharacteristic)
      {
         FillCharacteristic(evaluatedCharacteristic.Characteristic);
         FillCharacteristicActual(evaluatedCharacteristic.CharacteristicActual);
      }

      public string FeatureName { get; set; }
      public string CharacteristicName { get; set; }
      public string CharacteristicType { get; set; }
      public double? Nominal { get; set; }
      public double? UpperTolerance { get; set; }
      public double? LowerTolerance { get; set; }
      public double? Measured { get; set; }
      public double? Deviation { get; set; }
      public string Status { get; set; }
      public string Details { get; set; }

      private void FillCharacteristic(Characteristic characteristic)
      {
         Nominal = characteristic.Nominal;
         UpperTolerance = characteristic.UpperTolerance == null ? characteristic.ToleranceZone : characteristic.UpperTolerance;
         LowerTolerance = characteristic.LowerTolerance;
         FeatureName = characteristic.Feature?.Name;
         CharacteristicName = characteristic.Name;
         CharacteristicType =
            DataServiceLocalizationFinder.FindCharacteristicTypeName(characteristic.CharacteristicType);
         Details = DataServiceLocalizationFinder.FindCharacteristicDetailValue(characteristic.Detail);
      }

      private void FillCharacteristicActual(CharacteristicActual characteristicActual)
      {
         Measured = characteristicActual.Measured;
         Deviation = characteristicActual.Deviation;
         Status = Resources.ResourceManager.GetString(characteristicActual.Status.ToString()) ?? characteristicActual.Status.ToString();
      }
   }
}
