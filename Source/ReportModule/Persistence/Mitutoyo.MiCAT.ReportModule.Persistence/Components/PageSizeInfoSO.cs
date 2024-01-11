// <copyright file="PageSizeInfoSO.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Drawing.Printing;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Components
{
   public class PageSizeInfoSO
   {
      public PaperKind PaperKind { get; set; }

      public int Height { get; set; }

      public int Width { get; set; }
   }
}
