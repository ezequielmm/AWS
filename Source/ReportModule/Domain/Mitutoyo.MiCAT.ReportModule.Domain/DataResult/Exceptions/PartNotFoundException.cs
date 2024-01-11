// <copyright file="PartNotFoundException.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Mitutoyo.MiCAT.ReportModule.Domain.DataResult.Exceptions
{
   public class PartNotFoundException : ResultException
   {
      public PartNotFoundException(string errorMessage) : base(errorMessage, "PartNotFound")
      {
         Title = "PartNotFoundTitle";
      }
      public string Title { get; }
   }
}