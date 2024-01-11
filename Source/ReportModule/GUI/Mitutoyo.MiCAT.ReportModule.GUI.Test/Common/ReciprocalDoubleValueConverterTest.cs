// <copyright file="ReciprocalDoubleValueConverterTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.GUI.Common;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Common
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ReciprocalDoubleValueConverterTest
   {
      private ReciprocalScaleValueConverter _reciprocalScaleValueConverter;
      [SetUp]
      public void Setup()
      {
         _reciprocalScaleValueConverter = new ReciprocalScaleValueConverter();
      }
      [Test]
      public void ConvertCallShouldReturnInverseDoubleValue()
      {
         var value = _reciprocalScaleValueConverter.Convert((double)2.0, null, null, null);
         Assert.AreEqual(0.500, value);
      }
      [Test]
      public void ConvertBackCallShouldThrowException()
      {
         Assert.Catch<NotImplementedException>(()=> _reciprocalScaleValueConverter.ConvertBack((double) 2.0, null, null, null));
      }
   }
}
