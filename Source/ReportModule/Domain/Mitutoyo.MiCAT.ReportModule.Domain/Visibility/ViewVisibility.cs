// <copyright file="ViewVisibility.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.Common.Contract;

namespace Mitutoyo.MiCAT.ReportModule.Domain.Visibility
{
   public class ViewVisibility : BaseStateEntity<ViewVisibility>, IUnsaveableEntity
   {
      public ViewVisibility(ViewElement viewElement, bool isVisible)
         : this(Guid.NewGuid(), viewElement, isVisible)
      {
      }

      private ViewVisibility(Id<ViewVisibility> id, ViewElement viewElement, bool isVisible)
         : base(id)
      {
         ViewElement = viewElement;
         IsVisible = isVisible;
      }

      public ViewElement ViewElement { get; }
      public bool IsVisible { get; }

      public ViewVisibility WithVisible(bool isVisible)
      {
         return new ViewVisibility(Id, ViewElement, isVisible);
      }

      public ViewVisibility WithToggle()
      {
         return new ViewVisibility(Id, ViewElement, !IsVisible);
      }
   }
}
