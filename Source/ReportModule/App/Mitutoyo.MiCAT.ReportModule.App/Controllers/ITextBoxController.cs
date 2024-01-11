// <copyright file="ITextBoxController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public interface ITextBoxController
   {
      void ModifyText(Id<ReportTextBox> reportTextBoxId, string text);
      void AddTextboxToBody(int x, int y);
      void AddTextboxToSection(IItemId sectionId, int x, int y);
      void AddTextboxOnFakeSpace(int x, int y, int fakeSpaceStartPosition, int fakeSpaceHeight);
   }
}
