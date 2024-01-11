// <copyright file="ErrorResult.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Domain.DataResult
{
   public class ErrorResult: Result
   {
      public ErrorResult(string errorMessage) : base(ResultState.Error)
      {
         ErrorMessage = errorMessage;
      }
      public ErrorResult(string errorMessage, ResultErrorCode errorCode) : base(ResultState.Error)
      {
         ErrorMessage = errorMessage;
         ErrorCode = errorCode;
      }
      public string ErrorMessage { get; }
      public ResultErrorCode ErrorCode { get; }
   }
}
