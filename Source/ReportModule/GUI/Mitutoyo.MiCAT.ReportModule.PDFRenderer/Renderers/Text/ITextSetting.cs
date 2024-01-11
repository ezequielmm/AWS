// <copyright file="ITextSetting.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Windows;
using System.Windows.Media;
using TelerikFlow = Telerik.Windows.Documents.Fixed.Model.Editing.Flow;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.Text
{
   public interface ITextSetting
   {
      Brush Foreground { get; }
      FontFamily FontFamily { get; }
      FontStyle FontStyle { get; }
      FontWeight FontWeight { get; }
      double FontSize { get; }
      TextAlignment TextAlignment { get; }
      BaselineAlignment BaselineAlignment { get; }
      Color HighlightColor { get; }
      bool Underlined { get; }
      Color UnderlineColor();

      bool IsHighlighted { get; }

      bool SameAs(ITextSetting settingsToCompare);

      TelerikFlow.HorizontalAlignment TextAlignmentAsHorizontalAlignment();
      TelerikFlow.BaselineAlignment BaselineAlignmentAsTelerikFlow();
   }
}