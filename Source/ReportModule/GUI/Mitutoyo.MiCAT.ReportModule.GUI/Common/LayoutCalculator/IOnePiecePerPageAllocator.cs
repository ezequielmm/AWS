// <copyright file="IOnePiecePerPageAllocator.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator
{
   public interface IOnePiecePerPageAllocator
   {
      void AllocateEachPieceStartingNewPage(IVMMultiPageSplittableElement elementWithPieces, CommonPageLayout actualPageSettings, IVMPages pages, VMPage fromPage);
   }
}
