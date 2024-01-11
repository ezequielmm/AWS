// <copyright file="ILightDataGridCellContentFactory.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Columns;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Cells.Factories.Content
{
   public interface ILightDataGridCellContentFactory
   {
      FrameworkElement Create(
         LightDataGrid dataGrid,
         LightDataGridRow row,
         LightDataGridColumn column);
   }
}
