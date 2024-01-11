// <copyright file="MarginSO.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Domain;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Components
{
   public class MarginSO
   {
      //REFACTOR_VERSION: this version was upgrated from VERSION 1.0
      public MarginKind MarginKind { get; set; }
      public int Left { get; set; }
      public int Top { get; set; }
      public int Right { get; set; }
      public int Bottom { get; set; }
   }
}
