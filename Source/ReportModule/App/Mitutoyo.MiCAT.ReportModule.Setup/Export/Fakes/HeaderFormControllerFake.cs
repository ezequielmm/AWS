// <copyright file="HeaderFormControllerFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Export.Fakes
{
   public class HeaderFormControllerFake : IHeaderFormController
   {
      public void AddHeaderFormToBody(int x, int y)
      {
      }

      public void AddHeaderFormOnFakeSpace(int x, int y, int fakeSpaceStartPosition, int fakeSpaceHeight)
      {
      }

      public void AddHeaderFormToFooter(int x, int y)
      {
      }

      public void AddHeaderFormToHeader(int x, int y)
      {
      }

      public void AddHeaderFormToSection(IItemId sectionId, int x, int y)
      {
      }
   }
}