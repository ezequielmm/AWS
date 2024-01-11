﻿// <copyright file="MeasuredColumn.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView
{
   public class MeasuredColumn : Column
   {
      private const int DEFAULT_WIDTH = 65;
      public MeasuredColumn() : base("Measured", DEFAULT_WIDTH, "#0.00000", ContentAligment.Right) { }
   }
}
