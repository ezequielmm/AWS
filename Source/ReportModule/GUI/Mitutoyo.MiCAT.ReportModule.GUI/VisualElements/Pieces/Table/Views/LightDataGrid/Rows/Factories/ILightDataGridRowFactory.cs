// <copyright file="ILightDataGridRowFactory.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.Views.LightDataGrid.Rows.Factories
{
   public interface ILightDataGridRowFactory
   {
      LightDataGridRow Create(
         LightDataGrid dataGrid,
         int rowIndex,
         object itemSource = null);
   }
}
