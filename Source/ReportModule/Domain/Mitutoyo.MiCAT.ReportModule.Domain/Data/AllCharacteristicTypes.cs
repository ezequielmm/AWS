// <copyright file="AllCharacteristicTypes.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Immutable;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Common.Contract;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Data
{
   public class AllCharacteristicTypes : BaseStateEntity<AllCharacteristicTypes>, IUnsaveableEntity
   {
      public AllCharacteristicTypes(ImmutableList<string> characteristicTypes)
         : this(new Id<AllCharacteristicTypes>(new UniqueValue(Guid.NewGuid())), characteristicTypes)
      {
      }

      private AllCharacteristicTypes(Id<AllCharacteristicTypes> id, ImmutableList<string> characteristicTypes) : base(id)
      {
         CharacteristicTypes = characteristicTypes;
      }

      public ImmutableList<string> CharacteristicTypes { get; }
   }
}