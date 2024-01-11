// <copyright file="FeatureNameColumn.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView
{
   public class FeatureNameColumn : Column
   {
      private const int DEFAULT_WIDTH = 102;
      public FeatureNameColumn() : base("FeatureName", DEFAULT_WIDTH, string.Empty, ContentAligment.Left) { }
   }
}
