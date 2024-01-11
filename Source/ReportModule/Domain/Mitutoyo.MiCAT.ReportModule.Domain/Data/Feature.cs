// <copyright file="Feature.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Data
{
   public class Feature
   {
      public Guid? Id { get; set; }
      public string Name { get; set; }

      public Feature(Guid? id, string name)
      {
         Id = id;
         Name = name;
      }
   }
}
