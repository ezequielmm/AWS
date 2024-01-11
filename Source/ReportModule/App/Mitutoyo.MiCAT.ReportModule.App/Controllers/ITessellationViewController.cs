// <copyright file="ITessellationViewController.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModuleApp.Controllers
{
   public interface ITessellationViewController
   {
      void AddComponent(int y);
      void AddComponentOnFakeSpace(int y, int fakeSpaceStartPosition, int fakeSpaceHeight);
   }
}
