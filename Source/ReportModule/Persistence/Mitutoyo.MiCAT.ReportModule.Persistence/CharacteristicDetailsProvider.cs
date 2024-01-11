// <copyright file="CharacteristicDetailsProvider.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Immutable;

namespace Mitutoyo.MiCAT.ReportModule.Persistence
{
   public class CharacteristicDetailsProvider : ICharacteristicDetailsProvider
   {
      private string[] detailValues =
      {
         "DX",
         "DY",
         "DZ",
         "DR",
         "DA",
         "2D",
         "3D",
         "X-Axis",
         "Y-Axis",
         "Z-Axis",
         "X-Axis 2",
         "Y-Axis 2",
         "Z-Axis 2",
         "LS",
         "LC",
         "LL",
         "XY Plane",
         "SS",
         "SC",
         "SL",
         "Min",
         "Max",
         "Avg",
         "XY Ref. Plane",
         "YZ Ref. Plane",
         "ZX Ref. Plane",
         "XY Ref. Plane 2",
         "YZ Ref. Plane 2",
         "ZX Ref. Plane 2",
         "MMC",
         "LMC",
         "Diameter 2",
         "Radius 2"
      };

      public IImmutableList<string> GetAllCharacteristicDetails()
      {
         return detailValues.ToImmutableList();
      }
   }
}
