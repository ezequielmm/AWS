// <copyright file="Column.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Domain.Components.TableView
{
   public class Column
   {
      public Column(string name, double width, string format, ContentAligment contentAligment)
         : this(name, width, true, format, contentAligment)
      {
      }

      public Column(string name, double width, bool isVisible, string dataFormat, ContentAligment contentAligment)
      {
         Name = name;
         Width = width;
         IsVisible = isVisible;
         DataFormat = dataFormat;
         ContentAligment = contentAligment;
      }

      public string Name { get; }
      public bool IsVisible { get; }
      public string DataFormat { get; }
      public ContentAligment ContentAligment { get; }
      public double Width { get; }

      public Column WithIsVisible(bool isVisible)
      {
         return new Column(Name, Width, isVisible, DataFormat, ContentAligment);
      }

      public Column WithWidth(double width)
      {
         return new Column(Name, width, IsVisible, DataFormat, ContentAligment);
      }
   }
}
