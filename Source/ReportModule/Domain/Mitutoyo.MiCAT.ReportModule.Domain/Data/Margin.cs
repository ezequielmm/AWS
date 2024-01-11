// <copyright file="Margin.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Domain.Data
{
   public sealed class Margin
   {
      public Margin() : this(MarginKind.Normal, 0, 0, 0, 0) { }

      public Margin(MarginKind marginKind, int left, int top, int right, int bottom)
      {
         this.MarginKind = marginKind;
         this.Left = left;
         this.Top = top;
         this.Right = right;
         this.Bottom = bottom;
      }

      public int Left { get; }
      public int Top { get; }
      public int Right { get; }
      public int Bottom { get; }
      public MarginKind MarginKind { get; }

      public override string ToString() => $"{Left},{Top},{Right},{Bottom}";

      public override bool Equals(object obj)
      {
         return Equals(obj as Margin);
      }

      public bool Equals(Margin other)
      {
         return !(other is null)
            && other.Left == Left
            && other.Top == Top
            && other.Right == Right
            && other.Bottom == Bottom;
      }

      public override int GetHashCode()
      {
         return (Left, Top, Right, Bottom).GetHashCode();
      }

      public static bool operator ==(Margin m1, Margin m2)
      {
         return !(m1 is null) && !(m2 is null) && m1.Equals(m2);
      }

      public static bool operator !=(Margin m1, Margin m2)
      {
         return !(m1 == m2);
      }
   }
}
