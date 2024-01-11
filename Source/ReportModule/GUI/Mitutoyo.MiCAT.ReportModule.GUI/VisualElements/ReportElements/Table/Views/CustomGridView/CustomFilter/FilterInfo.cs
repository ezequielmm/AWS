// <copyright file="FilterInfo.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Linq;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Table.Views.CustomGridView.CustomFilter
{
   public class FilterInfo : IEquatable<FilterInfo>
   {
      public bool IsFilterable { get; set; }
      public string[] SelectedValues { get; set; }
      public FilterValueItem[] Values { get; set; }

      public override bool Equals(object obj)
      {
         return Equals(obj as FilterInfo);
      }

      public bool Equals(FilterInfo other)
      {
         return other != null
            && other.IsFilterable == IsFilterable
            && other.SelectedValues.SequenceEqual(SelectedValues)
            && other.Values.SequenceEqual(Values);
      }

      public override int GetHashCode()
      {
         return IsFilterable.GetHashCode() +
            SelectedValues.GetHashCode() +
            Values.GetHashCode();
      }

      public class FilterValueItem
      {
         public string DisplayText { get; set; }
         public string Value { get; set; }
         public bool IsDisabled { get; set; }

         public override bool Equals(object obj)
         {
            return Equals(obj as FilterValueItem);
         }

         public bool Equals(FilterValueItem other)
         {
            return other != null
                   && other.IsDisabled == IsDisabled
                   && other.DisplayText == DisplayText
                   && other.Value == Value;
         }

         public override int GetHashCode()
         {
            return DisplayText.GetHashCode() +
                   Value.GetHashCode() +
                   IsDisabled.GetHashCode();
         }
      }
   }
}
