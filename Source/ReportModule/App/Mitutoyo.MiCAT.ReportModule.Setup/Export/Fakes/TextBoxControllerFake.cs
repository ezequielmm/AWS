// <copyright file="TextBoxControllerFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Export.Fakes
{
   [ExcludeFromCodeCoverage]
   public class TextBoxControllerFake : ITextBoxController
   {
      public void AddTextboxToBody(int x, int y)
      {
      }

      public void AddTextboxOnFakeSpace(int x, int y, int fakeSpaceStartPosition, int fakeSpaceHeight)
      {
      }

      public void AddTextboxToFooter(int x, int y)
      {
      }

      public void AddTextboxToHeader(int x, int y)
      {
      }

      public void ModifyText(Id<ReportTextBox> reportTextBoxId, string text)
      {
      }

      public void AddTextboxToSection(IItemId sectionId, int x, int y)
      {
      }
   }
}