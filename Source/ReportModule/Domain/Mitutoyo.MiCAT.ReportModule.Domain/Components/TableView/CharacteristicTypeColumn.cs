// <copyright file="CharacteristicTypeColumn.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView
{
   public class CharacteristicTypeColumn : Column
   {
      private const int DEFAULT_WIDTH = 130;
      public CharacteristicTypeColumn() : base("CharacteristicType", DEFAULT_WIDTH, string.Empty, ContentAligment.Left) { }
   }
}
