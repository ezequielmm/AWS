// <copyright file="IRenderedData.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator
{
   public interface IRenderedData
   {
      IDisabledSpaceDataCollection DisabledSpaces { get; }
      IVMPages Pages { get; }
      bool IsFakeSpace(int visualY);
      bool IsInDisabledSpace(int visualY);
      bool IsInDisabledSpace(int visualY, DisabledSpaceData disabledSpaceToExclude);
      int ConvertToDomainY(int visualY);
      int ConvertToDomainY(int visualY, DisabledSpaceData disabledSpaceToExclude);
   }
}
