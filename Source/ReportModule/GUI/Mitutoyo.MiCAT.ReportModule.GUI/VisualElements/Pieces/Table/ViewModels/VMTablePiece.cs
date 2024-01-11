// <copyright file="VMTablePiece.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Mitutoyo.MiCAT.ReportModule.GUI.Properties;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces.Table.ViewModels
{
   public class VMTablePiece : VMVisualElementPiece
   {
      private const int HeightHeader = 35;
      private const int HeightEachRow = 29;

      private IEnumerable<VMEvaluatedCharacteristic> _dataToDisplay;
      private ColumnInfoList _columnInfos;

      public VMTablePiece(IVMVisualPlacement vmVisualPlacement, IVMReportComponent owner) : base(vmVisualPlacement, owner)
      {
      }

      public IEnumerable<VMEvaluatedCharacteristic> DataToDisplay
      {
         get => _dataToDisplay;
         set
         {
            _dataToDisplay = value;
            RaisePropertyChanged();
            VMPlacement.SetVisualSize(VMPlacement.VisualWidth, PixelsNeededForPartialSource());
         }
      }

      public ColumnInfoList ColumnInfos
      {
         get => _columnInfos;
         set
         {
            _columnInfos = value;
            RaisePropertyChanged();
         }
      }

      public int PieceIndex { get; set; }
      public int PiecesCount { get; set; }

      public string Label => string.Format(Resources.CharacteristicTable_PlaceHolder_FirstLine, PieceIndex, PiecesCount);

      private int PixelsNeededForPartialSource()
      {
         if (DataToDisplay == null)
            return HeightHeader;
         else
            return HeightHeader + (DataToDisplay.Count() * HeightEachRow);
      }
   }
}
