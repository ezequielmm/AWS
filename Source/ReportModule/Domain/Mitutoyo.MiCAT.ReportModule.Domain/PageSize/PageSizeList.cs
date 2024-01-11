// <copyright file="PageSizeList.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Xml.Linq;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.Domain
{
   public class PageSizeList : IPageSizeList
   {
      private List<PageSizeInfo> _pageSizeInfoList;

      public IEnumerable<PageSizeInfo> PageSizeInfoList => _pageSizeInfoList;

      public int ListCount => _pageSizeInfoList.Count;

      public PageSizeList()
      {
         _pageSizeInfoList = new List<PageSizeInfo>();
         Initialize();
      }

      public void Initialize()
      {
         OpenXmlFile openXmlFilel = new OpenXmlFile();
         XElement pageSizeTableXE = openXmlFilel.Load("Mitutoyo.MiCAT.ReportModule.Domain.PageSize.PageSizeTable.xml")
            .Element("PageSizeTable");
         IEnumerable<XElement> pageSizeXElements = pageSizeTableXE.Elements("PageSize");

         foreach (XElement pageSizeXE in pageSizeXElements)
         {
            AddPageSize(pageSizeXE);
         }
      }

      public PageSizeInfo FindPageSize(PaperKind paperKind)
      {
         return _pageSizeInfoList.Find(p => p.PaperKind == paperKind);
      }

      private void AddPageSize(XElement pageSizeXE)
      {
         var paperKindtext = pageSizeXE.Attribute("PaperKind").Value;
         var widthtext = pageSizeXE.Element("Width").Value;
         var heighttext = pageSizeXE.Element("Height").Value;

         var paperKind = (PaperKind) Enum.Parse(typeof(PaperKind), paperKindtext, true);
         var width = int.Parse(widthtext);
         var height = int.Parse(heighttext);
         _pageSizeInfoList.Add(new PageSizeInfo(paperKind, height, width));
      }
   }
}
