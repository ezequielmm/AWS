﻿// <copyright file="MultiPageElementManager.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElementList.ViewModels;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator
{
   public class MultiPageElementManager : IMultiPageElementManager
   {
      private IOnePiecePerPageAllocator _onePiecePerPageAllocator;

      public MultiPageElementManager(IOnePiecePerPageAllocator onePiecePerPageAllocator)
      {
         _onePiecePerPageAllocator = onePiecePerPageAllocator;
      }

      public void RenderMultiPageElement(IVMMultiPageSplittableElement elementToRender, CommonPageLayout actualPageSettings, IVMReportElementList elementList, IRenderedData renderedData)
      {
         elementList.RemovePieces(elementToRender.Pieces);
         elementToRender.RegeneratePieces();

         _onePiecePerPageAllocator.AllocateEachPieceStartingNewPage(elementToRender, actualPageSettings, renderedData.Pages, elementToRender.GetMainElementPage());

         elementList.AddPieces(elementToRender.Pieces);
      }
   }
}
