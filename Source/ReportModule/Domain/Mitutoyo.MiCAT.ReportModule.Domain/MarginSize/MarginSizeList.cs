// <copyright file="MarginSizeList.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.Domain
{
   public class MarginSizeList : IMarginSizeList
   {
      private List<Margin> _marginSizeInfoList;

      public IEnumerable<Margin> MarginSizeInfoList => _marginSizeInfoList;

      public int ListCount => _marginSizeInfoList.Count;

      public MarginSizeList()
      {
         _marginSizeInfoList = new List<Margin>();
         Initialize();
      }

      public void Initialize()
      {
         OpenXmlFile openXmlFilel = new OpenXmlFile();
         XElement marginSizeTableXE = openXmlFilel.Load("Mitutoyo.MiCAT.ReportModule.Domain.MarginSize.MarginSizeTable.xml")
            .Element("MarginSizeTable");
         IEnumerable<XElement> marginSizeXElements = marginSizeTableXE.Elements("MarginSize");

         foreach (XElement marginSizeXE in marginSizeXElements)
         {
            AddMarginSize(marginSizeXE);
         }
      }

      public Margin FindMarginSize(MarginKind marginKind)
      {
         return _marginSizeInfoList.Find(p => p.MarginKind == marginKind);
      }

      private void AddMarginSize(XElement marginSizeXE)
      {
         var marginKindtext = marginSizeXE.Attribute("MarginKind").Value;
         var top = int.Parse(marginSizeXE.Element("Top").Value);
         var bottom = int.Parse(marginSizeXE.Element("Bottom").Value);
         var left = int.Parse(marginSizeXE.Element("Left").Value);
         var right = int.Parse(marginSizeXE.Element("Right").Value);

         var marginKind = (MarginKind)Enum.Parse(typeof(MarginKind), marginKindtext, true);
         _marginSizeInfoList.Add(new Margin(marginKind, left, top, right, bottom));
      }
   }
   public enum MarginKind
   {
      None = 0,
      Narrow = 1,
      Normal =2
   }
}
