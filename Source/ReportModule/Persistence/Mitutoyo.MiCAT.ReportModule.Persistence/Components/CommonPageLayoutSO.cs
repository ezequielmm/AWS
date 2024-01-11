// <copyright file="CommonPageLayoutSO.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Components
{
   public class CommonPageLayoutSO
   {
      public Guid Id { get; set; }
      public PageSizeInfoSO PageSize { get; set; }
      public MarginSO CanvasMargin { get; set; }
      public HeaderSO Header { get; set; }
      public FooterSO Footer { get; set; }
   }
}
