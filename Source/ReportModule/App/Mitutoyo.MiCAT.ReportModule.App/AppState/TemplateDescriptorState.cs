// <copyright file="TemplateDescriptorState.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Common.Contract;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;

namespace Mitutoyo.MiCAT.ReportModuleApp.AppState
{
   public class TemplateDescriptorState : BaseStateEntity<TemplateDescriptorState>, IUnsaveableEntity
   {
      public TemplateDescriptorState() : this(new TemplateDescriptor())
      {
      }
      public TemplateDescriptorState(TemplateDescriptor templateDescriptor) : this(new Id<TemplateDescriptorState>(new UniqueValue(Guid.NewGuid())), templateDescriptor)
      {
      }

      private TemplateDescriptorState(Id<TemplateDescriptorState> id) : this(id, new TemplateDescriptor())
      {
      }
      private TemplateDescriptorState(Id<TemplateDescriptorState> id, TemplateDescriptor templateDescriptor) : base(id)
      {
         TemplateDescriptor = templateDescriptor;
      }

      public TemplateDescriptor TemplateDescriptor { get; }

      public TemplateDescriptorState With(TemplateDescriptor templateDescriptor)
      {
         return new TemplateDescriptorState(Id, templateDescriptor);
      }
   }
}
