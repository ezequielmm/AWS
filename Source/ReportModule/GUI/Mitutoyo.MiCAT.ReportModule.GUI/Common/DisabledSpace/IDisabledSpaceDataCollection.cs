// <copyright file="IDisabledSpaceDataCollection.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace
{
   public interface IDisabledSpaceDataCollection
   {
      IList<DisabledSpaceData> Items { get; }

      bool IsInDisabledSpace(int visualY);
      bool IsInDisabledSpace(int visualY, DisabledSpaceData disabledSpaceToExclude);
      IEnumerable<DisabledSpaceData> GetDisabledSpaceDataAffectingThisPosition(int visualY, DisabledSpaceData disabledSpaceToExclude);
      int TotalUsableSpaceTaken(int visualY, DisabledSpaceData disabledSpaceToExclude);
   }
}
