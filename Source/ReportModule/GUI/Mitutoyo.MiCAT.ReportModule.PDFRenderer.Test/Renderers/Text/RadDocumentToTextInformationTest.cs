// <copyright file="RadDocumentToTextInformationTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Mitutoyo.MiCAT.ReportModule.PDFRenderer.Renderers.Text;
using NUnit.Framework;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.FormatProviders.Xaml;
using Telerik.Windows.Documents.Model;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Test.Renderers.Text
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class RadDocumentToTextInformationTest
   {
      private RadDocument GetRadDocumentToTest()
      {
         var xamlformatProvider = new XamlFormatProvider();
         var radRichTextBox = new RadRichTextBox();
         var provider = new XamlDataProvider();

         provider.FormatProvider = xamlformatProvider;
         provider.RichTextBox = radRichTextBox;
         provider.Xaml = SerializedRadDocumentToTest;

         return radRichTextBox.Document;
      }

      [Test]
      [Apartment(ApartmentState.STA)]
      public void GetTextInformationShouldReturnTwoTextLineInfo()
      {
         //Assert
         var sut = new RadDocumentToTextInformation();

         //Act
         var result = sut.GetTextInformation(GetRadDocumentToTest());

         //Assert
         var lines = result.ToArray();

         Assert.AreEqual(2, lines.Length);
         Assert.AreEqual(16, lines[0].Count);
         Assert.AreEqual(5, lines[1].Count);
      }

      private const string SerializedRadDocumentToTest = @"<t:RadDocument xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:t='clr-namespace:Telerik.Windows.Documents.Model;assembly=Telerik.Windows.Documents' xmlns:s='clr-namespace:Telerik.Windows.Documents.Model.Styles;assembly=Telerik.Windows.Documents' xmlns:r='clr-namespace:Telerik.Windows.Documents.Model.Revisions;assembly=Telerik.Windows.Documents' xmlns:n='clr-namespace:Telerik.Windows.Documents.Model.Notes;assembly=Telerik.Windows.Documents' xmlns:th='clr-namespace:Telerik.Windows.Documents.Model.Themes;assembly=Telerik.Windows.Documents' LayoutMode='Flow' LineSpacing='1.15' LineSpacingType='Auto' ParagraphDefaultSpacingAfter='12' ParagraphDefaultSpacingBefore='0' StyleName='defaultDocumentStyle'>
                                                         <t:RadDocument.Captions>
                                                            <t:CaptionDefinition IsDefault = 'True' IsLinkedToHeading='False' Label='Figure' LinkedHeadingLevel='0' NumberingFormat='Arabic' SeparatorType='Hyphen' />
                                                            <t:CaptionDefinition IsDefault = 'True' IsLinkedToHeading='False' Label='Table' LinkedHeadingLevel='0' NumberingFormat='Arabic' SeparatorType='Hyphen' />
                                                         </t:RadDocument.Captions>
                                                         <t:RadDocument.ProtectionSettings>
                                                            <t:DocumentProtectionSettings EnableDocumentProtection = 'False' Enforce='False' HashingAlgorithm='None' HashingSpinCount='0' ProtectionMode='ReadOnly' />
                                                         </t:RadDocument.ProtectionSettings>
                                                         <t:RadDocument.Styles>
                                                            <s:StyleDefinition DisplayName = 'defaultDocumentStyle' IsCustom='False' IsDefault='False' IsPrimary='True' Name='defaultDocumentStyle' Type='Default'>
                                                            <s:StyleDefinition.ParagraphStyle>
                                                               <s:ParagraphProperties LineSpacing = '1.15' SpacingAfter='12' />
                                                            </s:StyleDefinition.ParagraphStyle>
                                                            <s:StyleDefinition.SpanStyle>
                                                               <s:SpanProperties FontFamily = 'Segoe UI' FontSize='12' FontStyle='Normal' FontWeight='Normal' />
                                                            </s:StyleDefinition.SpanStyle>
                                                            </s:StyleDefinition>
                                                            <s:StyleDefinition DisplayName = 'Normal' IsCustom='False' IsDefault='True' IsPrimary='True' Name='Normal' Type='Paragraph' UIPriority='0' />
                                                            <s:StyleDefinition DisplayName = 'Table Normal' IsCustom='False' IsDefault='True' IsPrimary='False' Name='TableNormal' Type='Table' UIPriority='59'>
                                                            <s:StyleDefinition.TableStyle>
                                                               <s:TableProperties CellPadding = '5,0,5,0' >
                                                                  <s:TableProperties.TableLook>
                                                                  <t:TableLook />
                                                                  </s:TableProperties.TableLook>
                                                               </s:TableProperties>
                                                            </s:StyleDefinition.TableStyle>
                                                            </s:StyleDefinition>
                                                         </t:RadDocument.Styles>
                                                         <t:Section>
                                                            <t:Paragraph>
                                                            <t:Paragraph.ParagraphSymbolPropertiesStyle>
                                                               <s:SpanProperties FlowDirection = 'LeftToRight' />
                                                            </t:Paragraph.ParagraphSymbolPropertiesStyle>
                                                            <t:Span Text = 'Testing ' />
                                                            <t:Span FontSize = '14.6666666666667' FontStyle='Italic' Text='conv' />
                                                            <t:Span FontSize = '14.6666666666667' FontStyle='Normal' Text='e' />
                                                            <t:Span FontSize = '14.6666666666667' FontStyle='Italic' Text='rting ' />
                                                            <t:Span Text = 't' />
                                                            <t:Span HighlightColor = '#FFFFFF00' Text='ext ' />
                                                            <t:Span FontWeight = 'Bold' HighlightColor='#FFFFFF00' Text='box t' />
                                                            <t:Span HighlightColor = '#FFFFFF00' Text='o' />
                                                            <t:Span Text = ' PDF.' />
                                                            </t:Paragraph>
                                                            <t:Paragraph>
                                                            <t:Span FontFamily = 'MV Boli' FontSize='16' Text='Diffe' />
                                                            <t:Span FontFamily = 'MV Boli' FontSize='16' Text='rent for' UnderlineDecoration='Line' />
                                                            <t:Span FontFamily = 'MV Boli' FontSize='16' Text='mats' />
                                                            </t:Paragraph>
                                                         </t:Section>
                                                      </t:RadDocument>";
   }
}
