// <copyright file="ImageControllerFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Export.Fakes
{
   public class ImageControllerFake : IImageController
   {
      public void AddImageToBody(int x, int y)
      {
      }

      public void AddImageOnFakeSpace(int x, int y, int fakeSpaceStartPosition, int fakeSpaceHeight)
      {
      }

      public void AddImageToFooter(int x, int y)
      {
      }

      public void AddImageToHeader(int x, int y)
      {
      }

      public void UpdateImage(Id<ReportImage> id, string content, int width, int height)
      {
      }

      public void AddImageToBoundarySection(IItemId sectionId, int x, int y)
      {
      }
   }
}