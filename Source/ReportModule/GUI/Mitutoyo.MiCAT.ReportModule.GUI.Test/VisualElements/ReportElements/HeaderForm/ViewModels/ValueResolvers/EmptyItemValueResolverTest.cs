// <copyright file="EmptyItemValueResolverTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels.ValueResolvers;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.HeaderForm.ViewModels.ValueResolvers
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class EmptyItemValueResolverTest
   {
      [Test]
      public void GetValueShouldBeAnEmptyString()
      {
         var resolver = new EmptyItemValueResolver();
         var result = resolver.GetValue();
         Assert.IsEmpty(result);
      }
   }
}
