// <copyright file="RenderedData.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator
{
   public class RenderedData : IRenderedData
   {
      public RenderedData(IDisabledSpaceDataCollection disabledSpaceDataCollection, IVMPages vmPages)
      {
         DisabledSpaces = disabledSpaceDataCollection;
         Pages = vmPages;
      }

      public IDisabledSpaceDataCollection DisabledSpaces { get; }
      public IVMPages Pages { get; }

      public bool IsFakeSpace(int visualY)
      {
         if (IsInDisabledSpace(visualY))
            return false;
         else
         {
            var currentPage = Pages.GetPageForPosition(visualY);

            return currentPage != null && currentPage.IsFakeSpace(visualY);
         }
      }

      public bool IsInDisabledSpace(int visualY)
      {
         return IsInDisabledSpace(visualY, null);
      }

      public bool IsInDisabledSpace(int visualY, DisabledSpaceData disabledSpaceToExclude)
      {
         return DisabledSpaces.IsInDisabledSpace(visualY, disabledSpaceToExclude);
      }

      public int ConvertToDomainY(int visualY)
      {
         return ConvertToDomainY(visualY, null);
      }
      public int ConvertToDomainY(int visualY, DisabledSpaceData disabledSpaceToExclude)
      {
         var currentPage = Pages.GetPageForPosition(visualY);

         if (currentPage == null)
            return Pages.GetDomainOutsidePages(visualY);
         else
            return visualY - currentPage.OffsetForComponentsOnThisPage - DisabledSpaces.TotalUsableSpaceTaken(visualY, disabledSpaceToExclude);
      }
   }
}
