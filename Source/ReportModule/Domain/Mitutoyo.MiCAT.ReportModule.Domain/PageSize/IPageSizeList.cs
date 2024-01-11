// <copyright file="IPageSizeList.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Drawing.Printing;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;

namespace Mitutoyo.MiCAT.ReportModule.Domain
{
   public interface IPageSizeList
   {
      IEnumerable<PageSizeInfo> PageSizeInfoList { get; }

      void Initialize();

      PageSizeInfo FindPageSize(PaperKind paperKind);
   }
}
