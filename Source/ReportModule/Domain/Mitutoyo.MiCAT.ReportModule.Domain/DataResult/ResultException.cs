// <copyright file="ResultException.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;

namespace Mitutoyo.MiCAT.ReportModule.Domain.DataResult
{
   public class ResultException : Exception
   {
      public string Key { get; set; }

      public ResultException(string message, string key) : base(message)
      {
         Key = key;
      }
      public ResultException(string key) : base()
      {
         Key = key;
      }
   }
}
