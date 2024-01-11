// <copyright file="DisabledSpaceDataCollection.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace
{
   public class DisabledSpaceDataCollection : IDisabledSpaceDataCollection
   {
      public DisabledSpaceDataCollection()
      {
         Items = new List<DisabledSpaceData>();
      }

      public IList<DisabledSpaceData> Items { get; }

      public bool IsInDisabledSpace(int visualY)
      {
         return IsInDisabledSpace(visualY, null);
      }
      public bool IsInDisabledSpace(int visualY, DisabledSpaceData disabledSpaceToExclude)
      {
         return GetDisabledSpaceDataInPosition(visualY, disabledSpaceToExclude).Any();
      }

      public IEnumerable<DisabledSpaceData> GetDisabledSpaceDataAffectingThisPosition(int visualY, DisabledSpaceData disabledSpaceToExclude)
      {
         return Items.Where(ds => ds.StartVisualY < visualY && ds != disabledSpaceToExclude);
      }

      public int TotalUsableSpaceTaken(int visualY, DisabledSpaceData disabledSpaceToExclude)
      {
         return GetDisabledSpaceDataAffectingThisPosition(visualY, disabledSpaceToExclude).Sum(ds => ds.UsableSpaceTaken);
      }
      private IEnumerable<DisabledSpaceData> GetDisabledSpaceDataInPosition(int visualY, DisabledSpaceData disabledSpaceToExclude)
      {
         return this.Items.Where(ds => visualY >= ds.StartVisualY && visualY <= ds.EndVisualY && ds != disabledSpaceToExclude);
      }
   }
}
