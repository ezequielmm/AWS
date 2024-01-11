// <copyright file="SuccessResult.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Domain.DataResult
{
   public class SuccessResult<T> : Result
   {
      public SuccessResult(T result) : base(ResultState.Success)
      {
         Result = result;
      }
      public T Result { get; }
   }

   public class SuccessResult : Result
   {
      public SuccessResult() : base(ResultState.Success)
      {
      }
   }
}
