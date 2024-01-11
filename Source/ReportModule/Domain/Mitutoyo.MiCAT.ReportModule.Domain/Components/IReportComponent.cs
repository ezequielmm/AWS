// <copyright file="IReportComponent.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Components
{
   public interface IReportComponent : IStateEntity, IValue
   {
      ReportComponentPlacement Placement { get; }
      IReportComponent WithPosition(int x, int y);
      IReportComponent WithSize(int widht, int height);
   }
}
