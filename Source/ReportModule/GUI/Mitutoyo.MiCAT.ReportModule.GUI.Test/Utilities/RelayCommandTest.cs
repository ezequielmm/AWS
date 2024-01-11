// <copyright file="RelayCommandTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Utilities
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class RelayCommandTest
   {
      public RelayCommand<object> SomeCommand { get; private set; }
      private void Execute(object param)
      {
         Executed = true;
      }
      public bool Executed { get; private set; }

      [Test]
      public void RelayCommand_ShouldBeExecutableNull()
      {
         SomeCommand = new RelayCommand<object>(Execute);

         Assert.IsTrue(SomeCommand.CanExecute(null));
      }

      [Test]
      public void RelayCommand_ShouldBeExecutableWithParameter()
      {
         SomeCommand = new RelayCommand<object>(Execute);

         object parameter = new object();

         Assert.IsTrue(SomeCommand.CanExecute(parameter));
      }
   }
}
