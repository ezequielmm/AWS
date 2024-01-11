// <copyright file="Result.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Domain.DataResult
{
   public class Result
   {
      private readonly ResultState _state;
      private readonly string _message;

      public Result(ResultState state, string message = "")
      {
         _state = state;
         _message = message;
      }

      public bool IsSuccess => _state == ResultState.Success;
      public bool IsError => _state == ResultState.Error;
      public string Message => _message;
   }

   public class Result<T> : Result
   {
      public Result(T resultObject, ResultInfo resultInfo) : base(resultInfo.ResultState, resultInfo.Message)
      {
         ResultObject = resultObject;
      }
      public T ResultObject { get; }
   }
   public class ResultInfo
   {
      public string Message { get; set; }
      public ResultState ResultState { get; set; }
      public ResultInfo(ResultState resultState, string message = "")
      {
         Message = message;
         ResultState = resultState;
      }
   }

   public enum ResultState
   {
      Success, Error
   }
   public enum ResultErrorCode
   {
      NotFound = 404,
      BadRequest = 400
   }
}