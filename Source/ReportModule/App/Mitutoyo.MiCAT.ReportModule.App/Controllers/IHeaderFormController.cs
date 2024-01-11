// <copyright file="IHeaderFormController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public interface IHeaderFormController
   {
      void AddHeaderFormToBody(int x, int y);
      void AddHeaderFormToSection(IItemId sectionId, int x, int y);
      void AddHeaderFormOnFakeSpace(int x, int y, int fakeSpaceStartPosition, int fakeSpaceHeight);
   }
}
