// <copyright file="ReportComponentsByPage.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Immutable;
using Mitutoyo.MiCAT.ReportModule.GUI.PageLayout;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.Containers.InteractiveControlContainer;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.ComponentsPageViewRelation
{
   public class ReportComponentsByPage
   {
      public ReportComponentsByPage(PageView page, IImmutableList<InteractiveControlContainer> components)
      {
         Page = page;
         Components = components;
      }

      public PageView Page { get; }
      public IImmutableList<InteractiveControlContainer> Components { get; }
   }
}
