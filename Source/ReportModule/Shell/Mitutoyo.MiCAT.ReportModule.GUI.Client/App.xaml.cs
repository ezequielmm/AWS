// <copyright file="App.xaml.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.GUI.Client
{
   using System.Diagnostics.CodeAnalysis;
   using Prism.Modularity;
   using Shell;

   /// <summary>
   /// Interaction logic for App.xaml
   /// </summary>
   [ExcludeFromCodeCoverage]
   public partial class App : ShellApplication
   {
      protected override IModuleCatalog CreateModuleCatalog()
      {
         return new ConfigurationModuleCatalog();
      }
   }
}
