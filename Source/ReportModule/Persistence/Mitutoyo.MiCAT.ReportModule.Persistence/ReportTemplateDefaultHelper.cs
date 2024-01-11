// <copyright file="ReportTemplateDefaultHelper.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template.CurrentVersion;
using Mitutoyo.MiCAT.Web.Data;
using Newtonsoft.Json;

namespace Mitutoyo.MiCAT.ReportModule.Persistence
{
   public class ReportTemplateDefaultHelper
   {
      private const string _blank = "<ReportTemplate>\r\n" +
                                    "   <CommonPageLayout>\r\n" +
                                    "      <Id>00000001-0000-0000-0000-000000000001</Id>\r\n" +
                                    "      <PageSize>\r\n" +
                                    "         <PaperKind>A4</PaperKind>\r\n" +
                                    "         <Height>1123</Height>\r\n" +
                                    "         <Width>794</Width>\r\n" +
                                    "      </PageSize>\r\n" +
                                    "      <CanvasMargin>\r\n" +
                                    "          <MarginKind>Normal</MarginKind><Left>0</Left> <Top>96</Top> <Right>0</Right> <Bottom>96</Bottom>" +
                                    "      </CanvasMargin>\r\n" +
                                    "      <Header>\r\n" +
                                    "          <Height>96</Height>" +
                                    "      </Header>\r\n" +
                                    "      <Footer>\r\n" +
                                    "          <Height>96</Height>" +
                                    "      </Footer>\r\n" +
                                    "   </CommonPageLayout>\r\n" +
                                    "   <CadLayouts>\r\n" +
                                    "   </CadLayouts>\r\n" +
                                    "</ReportTemplate>";
      private static IEnumerable<ReportTemplateMediumDTO> _reportTemplateDefaults = new List<ReportTemplateMediumDTO>() { GetBlank() };
      private static IEnumerable<ReportTemplateLowDTO> _reportTemplateDescriptorDefaults =
         new List<ReportTemplateLowDTO>()
         {
            new ReportTemplateLowDTO
            {
               Id = new Guid("9241b7ad-68e4-4095-9ee4-cdeed839eba8"),
               Name = "Blank",
               ReadOnly = true
            }
         };
      public static ReportTemplateMediumDTO GetBlank()
      {
         var reportTemplateData = new ReportTemplateDataSO();
         reportTemplateData.ReportTemplateContent = Regex.Unescape(_blank);
         reportTemplateData.VersionIdentifier = GetReportTemplateVersionIdentifierSO();

         return new ReportTemplateMediumDTO() { Id = new Guid("9241b7ad-68e4-4095-9ee4-cdeed839eba8"), Name = "Blank", ReadOnly = true, Template = JsonConvert.SerializeObject(reportTemplateData) };
      }

      private static VersionIdentifierSO GetReportTemplateVersionIdentifierSO()
      {
         var reportTemplateVersionIdentifier = new VersionIdentifierSO();
         reportTemplateVersionIdentifier.DataType = "ReportTemplate";
         reportTemplateVersionIdentifier.Version = new CurrentReportTemplateVersion().GetCurrentVersion();
         return reportTemplateVersionIdentifier;
      }
      public static IEnumerable<ReportTemplateLowDTO> GetReportTemplateDescriptorDefaults()
      {
         return _reportTemplateDescriptorDefaults;
      }
      public static IEnumerable<ReportTemplateMediumDTO> GetReportTemplateDefaults()
      {
         return _reportTemplateDefaults;
      }
      public static ReportTemplateMediumDTO GetReportTemplateDefaultById(Guid id)
      {
         return _reportTemplateDefaults.SingleOrDefault(x => x.Id == id);
      }
   }
}
