// <copyright file="PercentageInputValidator.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Text.RegularExpressions;

namespace Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.Zoom
{
   public class PercentageInputValidator
   {
      public class PercentageInputValidationResult
      {
         public bool IsValid { get; set; }
         public double PercentValue { get; set; }
      }

      private const string VALID_FORMAT = @"^(\d+)(\%?)$";
      private const string ALLOWED_CHARACTERS = "[0-9%]+";

      public PercentageInputValidationResult ValidateInput(string inputValue)
      {
         PercentageInputValidationResult result = new PercentageInputValidationResult();

         Match match = new Regex(VALID_FORMAT).Match(inputValue);
         result.IsValid = match.Success;

         if (match.Success)
         {
            int number = Convert.ToInt32(match.Groups[1].Value);
            result.PercentValue = number / 100d;
         }

         return result;
      }

      public bool IsAllowedCharacter(string text)
      {
         return new Regex(ALLOWED_CHARACTERS).Match(text).Success;
      }
   }
}
