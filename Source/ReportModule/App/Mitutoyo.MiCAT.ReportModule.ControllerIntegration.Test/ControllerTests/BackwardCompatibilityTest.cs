// <copyright file="BackwardCompatibilityTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Mitutoyo.MiCAT.Applications.Services.Core;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.DataServiceClient;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Persistence;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service.Helpers;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template;
using Mitutoyo.MiCAT.ReportModule.Persistence.Template.CurrentVersion;
using Mitutoyo.MiCAT.ReportModule.Setup.Configurations;
using Mitutoyo.MiCAT.ReportModule.Setup.Export.Fakes;
using Mitutoyo.MiCAT.ReportModuleApp.AppState;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Providers;
using Mitutoyo.MiCAT.ReportModuleApp.Providers.Inputs;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Mitutoyo.MiCAT.Web.Data;
using NUnit.Framework;
using Telerik.Windows.Documents.FormatProviders.Xaml;
using Telerik.Windows.Documents.Model;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule.ControllerIntegration.Test.ControllerTests
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class BackwardCompatibilityTest : BaseAppStateTest
   {
      private IMapper _mapper;
      private IReportTemplateSerializationManager _serializationManager;
      private IDataServiceClient _dataServiceClient;
      private IReportTemplateContentSerializerSelector _selector = new ReportTemplateContentSerializerSelector();
      private IReportTemplateVersionProvider _provider = new CurrentReportTemplateVersion();
      private IReportTemplateSerializer _serializer = new ReportTemplateSerializer();
      public IPersistenceServiceLocator _serviceLocator;
      public IUnityContainer _iOCContainer;
      private IReportTemplatePersistence _reportTemplatePersistence;
      private ITemplateNameResolver _templateNameResolver;
      private IUnsavedChangesService _unsavedChangesService;
      private IMessageNotifier _messageNotifier;
      private IReportTemplateDeleteConfirmationInput _reportTemplateConfirmationInput;
      private IReportTemplateController _reportTemplateController;

      public static ISnapShot BuildHelper(ISnapShot snapShot)
      {
         snapShot = snapShot.AddCollection<TemplateDescriptorState>(AppStateKinds.Undoable);

         return snapShot;
      }

      private void AssertComponent(IReportComponent component, int x, int y, int width, int height)
      {
         Assert.NotNull(component);

         var placement = component.Placement;

         Assert.AreEqual(x, placement.X);
         Assert.AreEqual(y, placement.Y);
         Assert.AreEqual(width, placement.Width);
         Assert.AreEqual(height, placement.Height);
      }

      private void AssertHeaderForm(ReportHeaderForm headerForm, int x, int y, int width, int height, string rowId)
      {
         AssertComponent(headerForm, x, y, width, height);
         Assert.AreEqual(Guid.Parse(rowId), headerForm.RowIds[0].Guid());
      }

      private void AssertImage(ReportImage image, int x, int y, int width, int height, int imgWidth, int imgHeight)
      {
         AssertComponent(image, x, y, width, height);
         using (var ms = new MemoryStream(Convert.FromBase64String(image.Image).ToArray()))
         {
            var img = Image.FromStream(ms);

            Assert.AreEqual(imgWidth, img.Width);
            Assert.AreEqual(imgHeight, img.Height);
         }
      }

      private void AssertTableColumn(ReportTableView table, int columnIndex, string featureName, int width, bool isVisible, string dataFormat, Domain.Components.TableView.ContentAligment contentAlignment)
      {
         Assert.AreEqual(featureName, table.Columns[columnIndex].Name);
         Assert.AreEqual(width, table.Columns[columnIndex].Width);
         Assert.AreEqual(isVisible, table.Columns[columnIndex].IsVisible);
         Assert.AreEqual(dataFormat, table.Columns[columnIndex].DataFormat);
         Assert.AreEqual(contentAlignment, table.Columns[columnIndex].ContentAligment);
      }
      private void AssertTable(ReportTableView table, int x, int y, int width, int height)
      {
         AssertComponent(table, x, y, width, height);

         AssertTableColumn(table, 0, "FeatureName", 102, true, string.Empty, Domain.Components.TableView.ContentAligment.Left);
         AssertTableColumn(table, 1, "CharacteristicType", 130, true, string.Empty, Domain.Components.TableView.ContentAligment.Left);
         AssertTableColumn(table, 2, "Details", 95, true, string.Empty, Domain.Components.TableView.ContentAligment.Left);
         AssertTableColumn(table, 3, "Nominal", 65, true, "#0.00000", Domain.Components.TableView.ContentAligment.Right);
         AssertTableColumn(table, 4, "UpperTolerance", 75, true, "#0.00000", Domain.Components.TableView.ContentAligment.Right);
         AssertTableColumn(table, 5, "LowerTolerance", 75, true, "#0.00000", Domain.Components.TableView.ContentAligment.Right);
         AssertTableColumn(table, 6, "Measured", 65, true, "#0.00000", Domain.Components.TableView.ContentAligment.Right);
         AssertTableColumn(table, 7, "Deviation", 65, true, "#0.00000", Domain.Components.TableView.ContentAligment.Right);
         AssertTableColumn(table, 8, "Status", 63, true, string.Empty, Domain.Components.TableView.ContentAligment.Left);
      }

      [SetUp]
      public void Setup()
      {
         SetUpHelper(BuildHelper);

         var configurationHelper = new AssemblyConfigurationHelper(Assembly.GetExecutingAssembly());
         var dsUri = configurationHelper.GetDataServiceApiUrl();
         _dataServiceClient = DataServiceClientFactory.CreateDataServiceClient(dsUri);

         _iOCContainer = new UnityContainer();
         _mapper = AutoMapperConfig.InitializeAutoMapper().CreateMapper();
         _serializationManager = new ReportTemplateSerializationManager(_mapper, _selector, _provider, _serializer);

         _iOCContainer.RegisterInstance(_dataServiceClient);
         _iOCContainer.RegisterInstance(_mapper);
         _serviceLocator = new PersistenceServiceLocator(_iOCContainer);

         _reportTemplatePersistence = new ReportTemplatePersistence(_serviceLocator, _mapper, _serializationManager);
         _templateNameResolver = new TemplateNameResolverFake();
         _unsavedChangesService = new UnsavedChangesServiceFake();
         _messageNotifier = new PdfBackgroundNotifierFake();
         _reportTemplateConfirmationInput = new ReportTemplateDeleteConfirmationInputFake(true);
         _reportTemplateController = new ReportTemplateController(_reportTemplatePersistence, _history, _templateNameResolver, _unsavedChangesService, _messageNotifier, _reportTemplateConfirmationInput);
      }

      [Test]
      [Category("Macro")]
      public async Task ReportTemplateV1_0_Can_Save_Load_Deserialize_Delete()
      {
         string templateFilename = "ControllerTests\\ReportTemplates\\Template_v1_0.json";
         var reportName = "ReportTemplateV1_0_Can_Save_Load_Deserialize_Delete Test " + DateTime.Now.Ticks;
         string template_v1_0 = File.ReadAllText(templateFilename);

         await _dataServiceClient.AddReportTemplate(new ReportTemplateCreateDTO()
         {
            Name = reportName,
            Template = template_v1_0
         });

         var templates = await _dataServiceClient.GetAllReportTemplates<ReportTemplateMediumDTO>();
         Assert.IsTrue(templates.Success.Any(t => t.Name == reportName));

         var template = templates.Success.First(t => t.Name == reportName);
         var report = await _reportTemplateController.GetReportTemplateById(template.Id);
         Assert.AreEqual(12, report.ReportComponentDataItems.Count());
         Assert.AreEqual(10, report.ReportComponents.Count());

         var components = report.ReportComponents.ToList();

         AssertHeaderForm(components[0] as ReportHeaderForm, 418, 203, 340, 32, "9ee0d282-4e8f-4740-a7f5-adbb644f94b7");
         AssertHeaderForm(components[1] as ReportHeaderForm, 48, 121, 340, 32, "06c91aa3-fe2a-4fe9-8675-953eb939ef5e");
         AssertHeaderForm(components[2] as ReportHeaderForm, 418, 162, 340, 32, "c239cc35-c8ea-4f89-9cad-657b07390641");
         AssertHeaderForm(components[3] as ReportHeaderForm, 48, 203, 340, 32, "eb5fff18-ffc5-48ed-91da-4b9a14ec1812");
         AssertHeaderForm(components[4] as ReportHeaderForm, 418, 121, 340, 32, "b78f9aa9-10e4-4536-bf6a-46462ed92cb1");
         AssertHeaderForm(components[5] as ReportHeaderForm, 48, 162, 340, 32, "79bc8108-4e57-4d03-8e48-d3ebb7fbe527");

         AssertImage(components[6] as ReportImage, 42, 8, 109, 53, 176, 88);

         AssertTable(components[7] as ReportTableView, 39, 429, 707, 54);

         var component8 = components[8] as ReportTextBox;
         AssertComponent(components[8] as ReportTextBox, 259, 32, 283, 34);
         Assert.AreEqual("<t:RadDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:t=\"clr-namespace:Telerik.Windows.Documents.Model;assembly=Telerik.Windows.Documents\" xmlns:s=\"clr-namespace:Telerik.Windows.Documents.Model.Styles;assembly=Telerik.Windows.Documents\" xmlns:r=\"clr-namespace:Telerik.Windows.Documents.Model.Revisions;assembly=Telerik.Windows.Documents\" xmlns:n=\"clr-namespace:Telerik.Windows.Documents.Model.Notes;assembly=Telerik.Windows.Documents\" xmlns:th=\"clr-namespace:Telerik.Windows.Documents.Model.Themes;assembly=Telerik.Windows.Documents\" version=\"1.4\" LayoutMode=\"Flow\" LineSpacing=\"1.15\" LineSpacingType=\"Auto\" ParagraphDefaultSpacingAfter=\"12\" ParagraphDefaultSpacingBefore=\"0\" StyleName=\"defaultDocumentStyle\">\n  <t:RadDocument.Captions>\n    <t:CaptionDefinition IsDefault=\"True\" IsLinkedToHeading=\"False\" Label=\"Figure\" LinkedHeadingLevel=\"0\" NumberingFormat=\"Arabic\" SeparatorType=\"Hyphen\" />\n    <t:CaptionDefinition IsDefault=\"True\" IsLinkedToHeading=\"False\" Label=\"Table\" LinkedHeadingLevel=\"0\" NumberingFormat=\"Arabic\" SeparatorType=\"Hyphen\" />\n  </t:RadDocument.Captions>\n  <t:RadDocument.ProtectionSettings>\n    <t:DocumentProtectionSettings EnableDocumentProtection=\"False\" Enforce=\"False\" HashingAlgorithm=\"None\" HashingSpinCount=\"0\" ProtectionMode=\"ReadOnly\" />\n  </t:RadDocument.ProtectionSettings>\n  <t:RadDocument.Styles>\n    <s:StyleDefinition DisplayName=\"Document Default Style\" IsCustom=\"False\" IsDefault=\"False\" IsPrimary=\"True\" Name=\"defaultDocumentStyle\" Type=\"Default\">\n      <s:StyleDefinition.ParagraphStyle>\n        <s:ParagraphProperties LineSpacing=\"1.15\" SpacingAfter=\"12\" />\n      </s:StyleDefinition.ParagraphStyle>\n      <s:StyleDefinition.SpanStyle>\n        <s:SpanProperties FontFamily=\"Segoe UI\" FontSize=\"12\" FontStyle=\"Normal\" FontWeight=\"Normal\" />\n      </s:StyleDefinition.SpanStyle>\n    </s:StyleDefinition>\n    <s:StyleDefinition DisplayName=\"Normal\" IsCustom=\"False\" IsDefault=\"True\" IsPrimary=\"True\" Name=\"Normal\" Type=\"Paragraph\" UIPriority=\"0\" />\n    <s:StyleDefinition DisplayName=\"Table Normal\" IsCustom=\"False\" IsDefault=\"True\" IsPrimary=\"False\" Name=\"TableNormal\" Type=\"Table\" UIPriority=\"59\">\n      <s:StyleDefinition.TableStyle>\n        <s:TableProperties CellPadding=\"5,0,5,0\">\n          <s:TableProperties.TableLook>\n            <t:TableLook />\n          </s:TableProperties.TableLook>\n        </s:TableProperties>\n      </s:StyleDefinition.TableStyle>\n    </s:StyleDefinition>\n  </t:RadDocument.Styles>\n  <t:Section>\n    <t:Paragraph TextAlignment=\"Center\">\n      <t:Span FontFamily=\"Segoe UI Semibold\" FontSize=\"21.3333333333333\" HighlightColor=\"#FFFFFFFF\" Text=\"Measurement Report\" />\n    </t:Paragraph>\n  </t:Section>\n</t:RadDocument>",
                        component8.Text);

         var complexTextBox = components[9] as ReportTextBox;
         AssertComponent(complexTextBox, 47, 258, 660, 149);
         Assert.AreEqual("<t:RadDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:t=\"clr-namespace:Telerik.Windows.Documents.Model;assembly=Telerik.Windows.Documents\" xmlns:s=\"clr-namespace:Telerik.Windows.Documents.Model.Styles;assembly=Telerik.Windows.Documents\" xmlns:r=\"clr-namespace:Telerik.Windows.Documents.Model.Revisions;assembly=Telerik.Windows.Documents\" xmlns:n=\"clr-namespace:Telerik.Windows.Documents.Model.Notes;assembly=Telerik.Windows.Documents\" xmlns:th=\"clr-namespace:Telerik.Windows.Documents.Model.Themes;assembly=Telerik.Windows.Documents\" version=\"1.4\" LayoutMode=\"Flow\" LineSpacing=\"1.15\" LineSpacingType=\"Auto\" ParagraphDefaultSpacingAfter=\"12\" ParagraphDefaultSpacingBefore=\"0\" StyleName=\"defaultDocumentStyle\">\n  <t:RadDocument.Captions>\n    <t:CaptionDefinition IsDefault=\"True\" IsLinkedToHeading=\"False\" Label=\"Figure\" LinkedHeadingLevel=\"0\" NumberingFormat=\"Arabic\" SeparatorType=\"Hyphen\" />\n    <t:CaptionDefinition IsDefault=\"True\" IsLinkedToHeading=\"False\" Label=\"Table\" LinkedHeadingLevel=\"0\" NumberingFormat=\"Arabic\" SeparatorType=\"Hyphen\" />\n  </t:RadDocument.Captions>\n  <t:RadDocument.ProtectionSettings>\n    <t:DocumentProtectionSettings EnableDocumentProtection=\"False\" Enforce=\"False\" HashingAlgorithm=\"None\" HashingSpinCount=\"0\" ProtectionMode=\"ReadOnly\" />\n  </t:RadDocument.ProtectionSettings>\n  <t:RadDocument.Styles>\n    <s:StyleDefinition DisplayName=\"defaultDocumentStyle\" IsCustom=\"False\" IsDefault=\"False\" IsPrimary=\"True\" Name=\"defaultDocumentStyle\" Type=\"Default\">\n      <s:StyleDefinition.ParagraphStyle>\n        <s:ParagraphProperties LineSpacing=\"1.15\" SpacingAfter=\"12\" />\n      </s:StyleDefinition.ParagraphStyle>\n      <s:StyleDefinition.SpanStyle>\n        <s:SpanProperties FontFamily=\"Segoe UI\" FontSize=\"12\" FontStyle=\"Normal\" FontWeight=\"Normal\" />\n      </s:StyleDefinition.SpanStyle>\n    </s:StyleDefinition>\n    <s:StyleDefinition DisplayName=\"Normal\" IsCustom=\"False\" IsDefault=\"True\" IsPrimary=\"True\" Name=\"Normal\" Type=\"Paragraph\" UIPriority=\"0\" />\n    <s:StyleDefinition DisplayName=\"Table Normal\" IsCustom=\"False\" IsDefault=\"True\" IsPrimary=\"False\" Name=\"TableNormal\" Type=\"Table\" UIPriority=\"59\">\n      <s:StyleDefinition.TableStyle>\n        <s:TableProperties CellPadding=\"5,0,5,0\">\n          <s:TableProperties.TableLook>\n            <t:TableLook />\n          </s:TableProperties.TableLook>\n        </s:TableProperties>\n      </s:StyleDefinition.TableStyle>\n    </s:StyleDefinition>\n  </t:RadDocument.Styles>\n  <t:Section>\n    <t:Paragraph>\n      <t:Paragraph.ParagraphSymbolPropertiesStyle>\n        <s:SpanProperties FlowDirection=\"LeftToRight\" />\n      </t:Paragraph.ParagraphSymbolPropertiesStyle>\n      <t:Span Text=\"Sample \" />\n      <t:Span FontWeight=\"Bold\" Text=\"Text Sample Text\" />\n    </t:Paragraph>\n    <t:Paragraph>\n      <t:Paragraph.ParagraphSymbolPropertiesStyle>\n        <s:SpanProperties FlowDirection=\"LeftToRight\" />\n      </t:Paragraph.ParagraphSymbolPropertiesStyle>\n      <t:Span FontFamily=\"Tahoma\" FontSize=\"16\" FontWeight=\"Bold\" ForeColor=\"#FFFF0000\" Text=\"Sample Text\" UnderlineDecoration=\"Line\" />\n      <t:Span Text=\" Sample Text\" />\n    </t:Paragraph>\n    <t:Paragraph TextAlignment=\"Center\">\n      <t:Paragraph.ParagraphSymbolPropertiesStyle>\n        <s:SpanProperties FlowDirection=\"LeftToRight\" />\n      </t:Paragraph.ParagraphSymbolPropertiesStyle>\n      <t:Span FontSize=\"30.6666666666667\" FontStyle=\"Italic\" ForeColor=\"#FF0070C0\" Text=\"Sample Text Sample Text\" />\n      <t:Span Text=\" Sample Text \" />\n      <t:Span FontSize=\"22.6666666666667\" HighlightColor=\"#FFFFFF00\" Text=\"Sample Text\" />\n    </t:Paragraph>\n    <t:Paragraph />\n  </t:Section>\n</t:RadDocument>",
                        complexTextBox.Text);

         var xamlformatProvider = new XamlFormatProvider();
         var doc = xamlformatProvider.Import(complexTextBox.Text);
         Assert.AreEqual(1, doc.Sections.Count());
         Assert.AreEqual(4, doc.EnumerateChildrenOfType<Paragraph>().ToList().Count);
         Assert.AreEqual(doc.EnumerateChildrenOfType<Paragraph>().First().EnumerateChildrenOfType<Span>().Last().Text, "Text Sample Text");

         var deleteReportTemplate = await _reportTemplateController.DeleteReportTemplate(template.Id);
         Assert.IsTrue(deleteReportTemplate);

         var deletedTemplate = await _dataServiceClient.GetReportTemplate<ReportTemplateMediumDTO>(template.Id);
         Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deletedTemplate.StatusCode);
      }

      [Test]
      [Category("Macro")]
      public async Task ReportTemplateV1_1_Can_Save_Load_Deserialize_Delete()
      {
         string templateFilename = "ControllerTests\\ReportTemplates\\Template_v1_1.json";
         var reportName = "ReportTemplateV1_1_Can_Save_Load_Deserialize_Delete Test " + DateTime.Now.Ticks;
         string template_v1_1 = File.ReadAllText(templateFilename);

         await _dataServiceClient.AddReportTemplate(new ReportTemplateCreateDTO()
         {
            Name = reportName,
            Template = template_v1_1
         });

         var templates = await _dataServiceClient.GetAllReportTemplates<ReportTemplateMediumDTO>();
         Assert.IsTrue(templates.Success.Any(t => t.Name == reportName));

         var template = templates.Success.First(t => t.Name == reportName);
         var report = await _reportTemplateController.GetReportTemplateById(template.Id);
         Assert.AreEqual(12, report.ReportComponentDataItems.Count());
         Assert.AreEqual(10, report.ReportComponents.Count());

         var components = report.ReportComponents.ToList();

         AssertHeaderForm(components[0] as ReportHeaderForm, 418, 121, 340, 32, "b78f9aa9-10e4-4536-bf6a-46462ed92cb1");
         AssertHeaderForm(components[1] as ReportHeaderForm, 48, 121, 340, 32, "06c91aa3-fe2a-4fe9-8675-953eb939ef5e");
         AssertHeaderForm(components[2] as ReportHeaderForm, 418, 162, 340, 32, "c239cc35-c8ea-4f89-9cad-657b07390641");
         AssertHeaderForm(components[3] as ReportHeaderForm, 48, 203, 340, 32, "eb5fff18-ffc5-48ed-91da-4b9a14ec1812");
         AssertHeaderForm(components[4] as ReportHeaderForm, 48, 162, 340, 32, "79bc8108-4e57-4d03-8e48-d3ebb7fbe527");
         AssertHeaderForm(components[5] as ReportHeaderForm, 418, 203, 340, 32, "9ee0d282-4e8f-4740-a7f5-adbb644f94b7");

         AssertImage(components[6] as ReportImage, 42, 8, 109, 53, 176, 88);

         AssertTable(components[7] as ReportTableView, 39, 429, 707, 54);

         var component8 = components[8] as ReportTextBox;
         AssertComponent(components[8] as ReportTextBox, 47, 258, 660, 149);
         Assert.AreEqual("<t:RadDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:t=\"clr-namespace:Telerik.Windows.Documents.Model;assembly=Telerik.Windows.Controls.RichTextBox\" xmlns:s=\"clr-namespace:Telerik.Windows.Documents.Model.Styles;assembly=Telerik.Windows.Controls.RichTextBox\" xmlns:r=\"clr-namespace:Telerik.Windows.Documents.Model.Revisions;assembly=Telerik.Windows.Controls.RichTextBox\" xmlns:n=\"clr-namespace:Telerik.Windows.Documents.Model.Notes;assembly=Telerik.Windows.Controls.RichTextBox\" xmlns:th=\"clr-namespace:Telerik.Windows.Documents.Model.Themes;assembly=Telerik.Windows.Controls.RichTextBox\" xmlns:sdt=\"clr-namespace:Telerik.Windows.Documents.Model.StructuredDocumentTags;assembly=Telerik.Windows.Controls.RichTextBox\" version=\"1.4\" LayoutMode=\"Flow\" LineSpacing=\"1.15\" LineSpacingType=\"Auto\" ParagraphDefaultSpacingAfter=\"12\" ParagraphDefaultSpacingBefore=\"0\" StyleName=\"defaultDocumentStyle\">\n  <t:RadDocument.Captions>\n    <t:CaptionDefinition IsDefault=\"True\" IsLinkedToHeading=\"False\" Label=\"Figure\" LinkedHeadingLevel=\"0\" NumberingFormat=\"Arabic\" SeparatorType=\"Hyphen\" />\n    <t:CaptionDefinition IsDefault=\"True\" IsLinkedToHeading=\"False\" Label=\"Table\" LinkedHeadingLevel=\"0\" NumberingFormat=\"Arabic\" SeparatorType=\"Hyphen\" />\n  </t:RadDocument.Captions>\n  <t:RadDocument.ProtectionSettings>\n    <t:DocumentProtectionSettings EnableDocumentProtection=\"False\" Enforce=\"False\" HashingAlgorithm=\"None\" HashingSpinCount=\"0\" ProtectionMode=\"ReadOnly\" />\n  </t:RadDocument.ProtectionSettings>\n  <t:RadDocument.Styles>\n    <s:StyleDefinition DisplayName=\"Document Default Style\" IsCustom=\"False\" IsDefault=\"False\" IsPrimary=\"True\" Name=\"defaultDocumentStyle\" Type=\"Default\">\n      <s:StyleDefinition.ParagraphStyle>\n        <s:ParagraphProperties LineSpacing=\"1.15\" SpacingAfter=\"12\" />\n      </s:StyleDefinition.ParagraphStyle>\n      <s:StyleDefinition.SpanStyle>\n        <s:SpanProperties FontFamily=\"Segoe UI\" FontSize=\"12\" FontStyle=\"Normal\" FontWeight=\"Normal\" ForeColor=\"#FF000000\" />\n      </s:StyleDefinition.SpanStyle>\n    </s:StyleDefinition>\n    <s:StyleDefinition DisplayName=\"Normal\" IsCustom=\"False\" IsDefault=\"True\" IsPrimary=\"True\" Name=\"Normal\" Type=\"Paragraph\" UIPriority=\"0\" />\n    <s:StyleDefinition DisplayName=\"Table Normal\" IsCustom=\"False\" IsDefault=\"True\" IsPrimary=\"False\" Name=\"TableNormal\" Type=\"Table\" UIPriority=\"59\">\n      <s:StyleDefinition.TableStyle>\n        <s:TableProperties CellPadding=\"5,0,5,0\">\n          <s:TableProperties.TableLook>\n            <t:TableLook />\n          </s:TableProperties.TableLook>\n        </s:TableProperties>\n      </s:StyleDefinition.TableStyle>\n    </s:StyleDefinition>\n  </t:RadDocument.Styles>\n  <t:Section>\n    <t:Paragraph>\n      <t:Paragraph.ParagraphSymbolPropertiesStyle>\n        <s:SpanProperties FlowDirection=\"LeftToRight\" />\n      </t:Paragraph.ParagraphSymbolPropertiesStyle>\n      <t:Span Text=\"Sample \" />\n      <t:Span FontWeight=\"Bold\" Text=\"Text Sample Text\" />\n    </t:Paragraph>\n    <t:Paragraph>\n      <t:Paragraph.ParagraphSymbolPropertiesStyle>\n        <s:SpanProperties FlowDirection=\"LeftToRight\" />\n      </t:Paragraph.ParagraphSymbolPropertiesStyle>\n      <t:Span FontFamily=\"Tahoma\" FontSize=\"16\" FontWeight=\"Bold\" ForeColor=\"#FFFF0000\" Text=\"Sample Text\" UnderlineDecoration=\"Line\" />\n      <t:Span Text=\" Sample Text\" />\n    </t:Paragraph>\n    <t:Paragraph TextAlignment=\"Center\">\n      <t:Paragraph.ParagraphSymbolPropertiesStyle>\n        <s:SpanProperties FlowDirection=\"LeftToRight\" />\n      </t:Paragraph.ParagraphSymbolPropertiesStyle>\n      <t:Span FontSize=\"30.6666666666667\" FontStyle=\"Italic\" ForeColor=\"#FF0070C0\" Text=\"Sample Text Sample Text\" />\n      <t:Span Text=\" Sample Text \" />\n      <t:Span FontSize=\"22.6666666666667\" HighlightColor=\"#FFFFFF00\" Text=\"Sample Text v2.0\" />\n    </t:Paragraph>\n    <t:Paragraph />\n  </t:Section>\n</t:RadDocument>", component8.Text);

         var deleteReportTemplate = await _reportTemplateController.DeleteReportTemplate(template.Id);
         Assert.IsTrue(deleteReportTemplate);

         var deletedTemplate = await _dataServiceClient.GetReportTemplate<ReportTemplateMediumDTO>(template.Id);
         Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deletedTemplate.StatusCode);
      }

      [Test]
      [Category("Macro")]
      public async Task ReportTemplate_OutOfTheBox_Can_Load_Deserialize()
      {
         string outOfTheBoxTemplateName = "LocalKey_MiCATTemplate2";
         var reportName = "ReportTemplate_OutOfTheBox_Can_Load_Deserialize Test " + DateTime.Now.Ticks;

         var templates = await _dataServiceClient.GetAllReportTemplates<ReportTemplateMediumDTO>();
         Assert.IsTrue(templates.Success.Any(t => t.Name == outOfTheBoxTemplateName));

         var template = templates.Success.First(t => t.Name == outOfTheBoxTemplateName);
         var report = await _reportTemplateController.GetReportTemplateById(template.Id);
         Assert.IsTrue(report.ReportComponentDataItems.Count() == 14);
         Assert.IsTrue(report.ReportComponents.Count() == 13);

         var components = report.ReportComponents.ToList();

         AssertHeaderForm(components[0] as ReportHeaderForm, 48, 244, 340, 32, "9ee0d282-4e8f-4740-a7f5-adbb644f94b7");
         AssertHeaderForm(components[1] as ReportHeaderForm, 48, 121, 340, 32, "06c91aa3-fe2a-4fe9-8675-953eb939ef5e");
         AssertHeaderForm(components[2] as ReportHeaderForm, 418, 203, 340, 32, "c239cc35-c8ea-4f89-9cad-657b07390641");
         AssertHeaderForm(components[3] as ReportHeaderForm, 48, 203, 340, 32, "eb5fff18-ffc5-48ed-91da-4b9a14ec1812");
         AssertHeaderForm(components[4] as ReportHeaderForm, 418, 121, 340, 32, "c5aed39e-febf-445c-88c2-168dc52b449a");
         AssertHeaderForm(components[5] as ReportHeaderForm, 418, 162, 340, 32, "b78f9aa9-10e4-4536-bf6a-46462ed92cb1");
         AssertHeaderForm(components[6] as ReportHeaderForm, 48, 162, 340, 32, "79bc8108-4e57-4d03-8e48-d3ebb7fbe527");

         AssertImage(components[7] as ReportImage, 42, 8, 109, 53, 176, 88);

         AssertImage(components[8] as ReportImage, 713, 4, 41, 64, 76, 112);

         AssertTable(components[9] as ReportTableView, 44, 346, 707, 54);

         var component10 = components[10] as ReportTextBox;
         AssertComponent(components[10] as ReportTextBox, 41, 289, 728, 24);
         Assert.AreEqual("<t:RadDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:t=\"clr-namespace:Telerik.Windows.Documents.Model;assembly=Telerik.Windows.Documents\" xmlns:s=\"clr-namespace:Telerik.Windows.Documents.Model.Styles;assembly=Telerik.Windows.Documents\" xmlns:r=\"clr-namespace:Telerik.Windows.Documents.Model.Revisions;assembly=Telerik.Windows.Documents\" xmlns:n=\"clr-namespace:Telerik.Windows.Documents.Model.Notes;assembly=Telerik.Windows.Documents\" xmlns:th=\"clr-namespace:Telerik.Windows.Documents.Model.Themes;assembly=Telerik.Windows.Documents\" version=\"1.4\" LayoutMode=\"Flow\" LineSpacing=\"1.15\" LineSpacingType=\"Auto\" ParagraphDefaultSpacingAfter=\"12\" ParagraphDefaultSpacingBefore=\"0\" StyleName=\"defaultDocumentStyle\">\n  <t:RadDocument.Captions>\n    <t:CaptionDefinition IsDefault=\"True\" IsLinkedToHeading=\"False\" Label=\"Figure\" LinkedHeadingLevel=\"0\" NumberingFormat=\"Arabic\" SeparatorType=\"Hyphen\" />\n    <t:CaptionDefinition IsDefault=\"True\" IsLinkedToHeading=\"False\" Label=\"Table\" LinkedHeadingLevel=\"0\" NumberingFormat=\"Arabic\" SeparatorType=\"Hyphen\" />\n  </t:RadDocument.Captions>\n  <t:RadDocument.ProtectionSettings>\n    <t:DocumentProtectionSettings EnableDocumentProtection=\"False\" Enforce=\"False\" HashingAlgorithm=\"None\" HashingSpinCount=\"0\" ProtectionMode=\"ReadOnly\" />\n  </t:RadDocument.ProtectionSettings>\n  <t:RadDocument.Styles>\n    <s:StyleDefinition DisplayName=\"defaultDocumentStyle\" IsCustom=\"False\" IsDefault=\"False\" IsPrimary=\"True\" Name=\"defaultDocumentStyle\" Type=\"Default\">\n      <s:StyleDefinition.ParagraphStyle>\n        <s:ParagraphProperties LineSpacing=\"1.15\" SpacingAfter=\"12\" />\n      </s:StyleDefinition.ParagraphStyle>\n      <s:StyleDefinition.SpanStyle>\n        <s:SpanProperties FontFamily=\"Segoe UI\" FontSize=\"12\" FontStyle=\"Normal\" FontWeight=\"Normal\" />\n      </s:StyleDefinition.SpanStyle>\n    </s:StyleDefinition>\n    <s:StyleDefinition DisplayName=\"Normal\" IsCustom=\"False\" IsDefault=\"True\" IsPrimary=\"True\" Name=\"Normal\" Type=\"Paragraph\" UIPriority=\"0\" />\n    <s:StyleDefinition DisplayName=\"Table Normal\" IsCustom=\"False\" IsDefault=\"True\" IsPrimary=\"False\" Name=\"TableNormal\" Type=\"Table\" UIPriority=\"59\">\n      <s:StyleDefinition.TableStyle>\n        <s:TableProperties CellPadding=\"5,0,5,0\">\n          <s:TableProperties.TableLook>\n            <t:TableLook />\n          </s:TableProperties.TableLook>\n        </s:TableProperties>\n      </s:StyleDefinition.TableStyle>\n    </s:StyleDefinition>\n  </t:RadDocument.Styles>\n  <t:Section>\n    <t:Paragraph>\n      <t:Span HighlightColor=\"#FFEEECE1\" Text=\"                                                                                                                                                                                                                        \" />\n    </t:Paragraph>\n  </t:Section>\n</t:RadDocument>",
                        component10.Text);
      }
   }
}
