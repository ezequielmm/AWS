﻿// <copyright file="IPdfExportRegistrar.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.Utilities.IoC;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Export
{
   public interface IPdfExportRegistrar
   {
      void Register(IServiceRegistrar serviceRegistrar);
   }
}
