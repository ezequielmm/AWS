// <copyright file="IVMMultiPageSplittableElement.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Immutable;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Pieces;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements
{
   public interface IVMMultiPageSplittableElement : IVMVisualElement
   {
      IImmutableList<IVMVisualElementPiece> Pieces { get; }
      void RegeneratePieces();
      VMPage GetMainElementPage();
   }
}
