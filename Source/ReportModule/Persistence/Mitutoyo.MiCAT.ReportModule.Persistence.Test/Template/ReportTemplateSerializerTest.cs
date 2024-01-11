// <copyright file="ReportTemplateSerializerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Test.Template
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReportTemplateSerializerTest
   {
      private const string SERIALIZED_TEMPLATE_CONTENT = "<ReportTemplate><CommonPageLayout><Id>00000001-0000-0000-0000-000000000001</Id><PageSize><PaperKind>Letter</PaperKind><Height>1056</Height><Width>816</Width></PageSize><CanvasMargin><Left>60</Left> <Top>30</Top> <Right>30</Right> <Bottom>30</Bottom></CanvasMargin></CommonPageLayout><CadLayouts></CadLayouts><ReportComponents><TextBox><Id>2eea57d1-ac99-4f4e-9e5f-7c3d8c0579bb</Id><X>0</X><Y>0</Y><Text>Text Content</Text><Width>200</Width><Height>100</Height></TextBox></ReportComponents></ReportTemplate>";
      private string serializedTemplate = "{\"VersionIdentifier\":{\"Version\":\"1.0\",\"DataType\":\"ReportTemplate\"},\"ReportTemplateContent\":\"" + SERIALIZED_TEMPLATE_CONTENT + "\"}";

      [Test]
      public void DeserializeTest()
      {
         // Arrange
         var serializer = new ReportTemplateSerializer();

         // Act
         var result = serializer.Deserialize(serializedTemplate);
         var resultTemplate = (result as SuccessResult<ReportTemplateDataSO>).Result;

         // Assert
         Assert.AreEqual(resultTemplate.VersionIdentifier.Version, "1.0");
         Assert.AreEqual(resultTemplate.VersionIdentifier.DataType, "ReportTemplate");
         Assert.AreEqual(resultTemplate.ReportTemplateContent, SERIALIZED_TEMPLATE_CONTENT);
      }

      [Test]
      public void SerializeTest()
      {
         // Arrange
         var templateSO = new ReportTemplateDataSO();
         templateSO.ReportTemplateContent = SERIALIZED_TEMPLATE_CONTENT;
         templateSO.VersionIdentifier = new VersionIdentifierSO()
         {
            DataType = "ReportTemplate",
            Version = "1.0"
         };

         var serializer = new ReportTemplateSerializer();

         // Act
         var result = (serializer.Serialize(templateSO) as SuccessResult<string>).Result;

         // Assert
         Assert.AreEqual(result, serializedTemplate);
      }
   }
}
