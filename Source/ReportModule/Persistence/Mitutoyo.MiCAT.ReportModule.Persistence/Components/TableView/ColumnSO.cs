// <copyright file="ColumnSO.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Components.TableView
{
   public class ColumnSO
   {
      public string Name { get; set; }
      public bool IsVisible { get; set; }
      public double Width { get; set; }
      public string DataFormat { get; set; }
      public ContentAligment ContentAligment { get; set; }
   }
}
