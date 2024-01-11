// <copyright file="DynamicPropertiesState.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Immutable;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Common.Contract;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;

namespace Mitutoyo.MiCAT.ReportModuleApp.AppState
{
   public class DynamicPropertiesState : BaseStateEntity<DynamicPropertiesState>, IUnsaveableEntity
   {
      public IImmutableList<DynamicPropertyDescriptor> DynamicProperties { get; }

      public DynamicPropertiesState(Id<DynamicPropertiesState> id)
         : base(id)
      {
         DynamicProperties = ImmutableList.Create<DynamicPropertyDescriptor>();
      }

      public DynamicPropertiesState(
         Id<DynamicPropertiesState> id,
         IImmutableList<DynamicPropertyDescriptor> dynamicProperties
         )
         : base(id)
      {
         DynamicProperties = dynamicProperties;
      }

      public DynamicPropertiesState With(IImmutableList<DynamicPropertyDescriptor> dynamicProperties)
      {
         return new DynamicPropertiesState(Id, dynamicProperties);
      }
   }
}
