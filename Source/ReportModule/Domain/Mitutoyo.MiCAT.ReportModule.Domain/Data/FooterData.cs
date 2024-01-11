// <copyright file="FooterData.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Domain.Data
{
   public class FooterData
   {
      public int Height { get; }

      public FooterData() : this(0) { }

      public FooterData(int height)
      {
         Height = height;
      }
   }
}
