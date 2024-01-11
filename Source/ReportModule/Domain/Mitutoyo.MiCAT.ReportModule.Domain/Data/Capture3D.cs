// <copyright file="Capture3D.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Drawing;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Data
{
   public class Capture3D
   {
      public Capture3D(string name, Image image, int index)
      {
         Name = name;
         Image = image;
         Index = index;
      }

      public string Name { get; }
      public int Index { get; }
      public Image Image { get; }
      public bool IsValid
      {
         get
         {
            return Image != null;
         }
      }
   }
}
