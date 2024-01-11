// <copyright file="ResultHelperTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Test.DataResult
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ResultHelperTest
   {
      [Test]
      public void ThrowIfFailureWithKeyShouldThrowKey()
      {
         var result = new ErrorResult("error");
         var key = "Key";
         var ex = (ResultException)Assert.Catch(() => ResultHelper.ThrowIfFailure(result, key));
         Assert.AreEqual(ex.Key, key);
      }
      [Test]
      public void ThrowIfFailureWithFactoryShouldThrowDefault()
      {
         var result = new ErrorResult("error");
         var message = "Message";
         var key = "Key";
         var ex = (ResultException)Assert.Catch(() => ResultHelper.ThrowIfFailure(result, (code) =>
         {
            switch (code) { case ResultErrorCode.BadRequest: throw new ResultException("one", "one"); default: throw new ResultException(message, key); }
         }));
         Assert.AreEqual(ex.Key, key);
      }
      [Test]
      public void ThrowIfFailureWithResultExceptionShouldThrowThatException()
      {
         var result = new ErrorResult("error");
         var exception = new ResultException("Exception", "Exception");
         var ex = (ResultException)Assert.Catch(() => ResultHelper.ThrowIfFailure(result, exception));
         Assert.AreEqual(exception, ex);
      }
   }
}
