// <copyright file="IVMReportElementList.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Immutable;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElementList.ViewModels
{
   public interface IVMReportElementList
   {
      ImmutableList<IVMReportComponent> Elements { get;  }
      IImmutableList<IVMVisualElementPiece> ElementPieces { get;  }

      void RemovePieces(IImmutableList<IVMVisualElementPiece> piecesToRemove);
      void AddPieces(IImmutableList<IVMVisualElementPiece> piecesToAdd);
      event EventHandler<LayoutRecalculationNeededEventArgs> LayoutRecalculationNeeded;
   }
}
