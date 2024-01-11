// <copyright file="DisabledSpaceData.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.DisabledSpace
{
   public class DisabledSpaceData
   {
      public DisabledSpaceData (int startDomainY, VMPage startPage, int startVisualY, VMPage endPage, int endVisualY, int usableSpaceTaken)
      {
         StartDomainY = startDomainY;
         StartPage = startPage;
         StartVisualY = startVisualY;
         EndPage = endPage;
         EndVisualY = endVisualY;
         UsableSpaceTaken = usableSpaceTaken;
      }

      public VMPage StartPage { get; }
      public int StartDomainY { get; }
      public int StartVisualY { get; }
      public VMPage EndPage { get; }
      public int EndVisualY { get; }
      public int UsableSpaceTaken { get; }

      public bool IsAffectedByDisabledSpace(int domainY)
      {
         return (StartDomainY < domainY);
      }
   }
}
