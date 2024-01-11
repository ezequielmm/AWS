// <copyright file="XAMLConverter.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.TextBox.Views
{
   [ExcludeFromCodeCoverage]
   public static class XAMLConverter
   {
      public static string ToXAML(string text)
      {
         return $@"<t:RadDocument xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:t='clr-namespace:Telerik.Windows.Documents.Model;assembly=Telerik.Windows.Documents' xmlns:s='clr-namespace:Telerik.Windows.Documents.Model.Styles;assembly=Telerik.Windows.Documents' xmlns:r='clr-namespace:Telerik.Windows.Documents.Model.Revisions;assembly=Telerik.Windows.Documents' xmlns:n='clr-namespace:Telerik.Windows.Documents.Model.Notes;assembly=Telerik.Windows.Documents' xmlns:th='clr-namespace:Telerik.Windows.Documents.Model.Themes;assembly=Telerik.Windows.Documents' LayoutMode='Flow' LineSpacing='1.15' LineSpacingType='Auto' ParagraphDefaultSpacingAfter='12' ParagraphDefaultSpacingBefore='0' StyleName='defaultDocumentStyle'>
           <t:RadDocument.Captions>
             <t:CaptionDefinition IsDefault='True' IsLinkedToHeading='False' Label='Figure' LinkedHeadingLevel='0' NumberingFormat='Arabic' SeparatorType='Hyphen' />
             <t:CaptionDefinition IsDefault='True' IsLinkedToHeading='False' Label='Table' LinkedHeadingLevel='0' NumberingFormat='Arabic' SeparatorType='Hyphen' />
           </t:RadDocument.Captions>
           <t:RadDocument.ProtectionSettings>
             <t:DocumentProtectionSettings EnableDocumentProtection='False' Enforce='False' HashingAlgorithm='None' HashingSpinCount='0' ProtectionMode='ReadOnly' />
           </t:RadDocument.ProtectionSettings>
           <t:RadDocument.Styles>
             <s:StyleDefinition DisplayName='defaultDocumentStyle' IsCustom='False' IsDefault='False' IsPrimary='True' Name='defaultDocumentStyle' Type='Default'>
               <s:StyleDefinition.ParagraphStyle>
                 <s:ParagraphProperties LineSpacing='1.15' SpacingAfter='12' />
               </s:StyleDefinition.ParagraphStyle>
               <s:StyleDefinition.SpanStyle>
                 <s:SpanProperties FontFamily='Arial' FontSize='12' FontStyle='Normal' FontWeight='Normal' />
               </s:StyleDefinition.SpanStyle>
             </s:StyleDefinition>
             <s:StyleDefinition DisplayName='Normal' IsCustom='False' IsDefault='True' IsPrimary='True' Name='Normal' Type='Paragraph' UIPriority='0' />
             <s:StyleDefinition DisplayName='Table Normal' IsCustom='False' IsDefault='True' IsPrimary='False' Name='TableNormal' Type='Table' UIPriority='59'>
               <s:StyleDefinition.TableStyle>
                 <s:TableProperties CellPadding='5,0,5,0'>
                   <s:TableProperties.TableLook>
                     <t:TableLook />
                   </s:TableProperties.TableLook>
                 </s:TableProperties>
               </s:StyleDefinition.TableStyle>
             </s:StyleDefinition>
           </t:RadDocument.Styles>
           <t:Section>
             <t:Paragraph>
               <t:Span Text='{text}' />
             </t:Paragraph>
           </t:Section>
         </t:RadDocument>";
      }
   }
}
