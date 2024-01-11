// <copyright file="ResultHelper.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>
using System;
namespace Mitutoyo.MiCAT.ReportModule.Domain.DataResult
{
   public static class ResultHelper
   {
      private const string GenericErrorMessage = "Unexpected error.";
      private const string GenericErrorResourceKey = "GenericError";
      private const string GenericNotFoundErrorMessage = "Resource not found.";
      private const string GenericNotFoundErrorResourceKey = "GenericNotFoundError";
      public static void ThrowIfFailure(Result result, Func<ResultErrorCode, ResultException> resultExceptionFactory)
      {
         if (!result.IsSuccess)
         {
            if (result is ErrorResult errorResult)
            {
               throw resultExceptionFactory(errorResult.ErrorCode);
            }
            throw new ResultException(result.Message, GenericErrorResourceKey);
         }
      }
      public static void ThrowIfFailure(Result result, string errorKey)
      {
         if (!result.IsSuccess)
         {
            if (result is ErrorResult errorResult)
            {
               throw new ResultException(errorResult.ErrorMessage, errorKey);
            }
            throw new ResultException(result.Message, GenericErrorResourceKey);
         }
      }
      public static void ThrowIfFailure(Result result, ResultException resultException)
      {
         if (!result.IsSuccess)
         {
            if (result is ErrorResult errorResult)
            {
               throw resultException;
            }
            throw new ResultException(result.Message, GenericErrorResourceKey);
         }
      }
   }
}