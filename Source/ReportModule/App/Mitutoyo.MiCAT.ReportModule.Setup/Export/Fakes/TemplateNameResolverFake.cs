﻿// <copyright file="TemplateNameResolverFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModuleApp.Providers;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Export.Fakes
{
   [ExcludeFromCodeCoverage]
   public class TemplateNameResolverFake : ITemplateNameResolver
   {
      public TemplateNameResult QueryTemplateName()
      {
         return new TemplateNameResult(string.Empty, true);
      }
   }
}
