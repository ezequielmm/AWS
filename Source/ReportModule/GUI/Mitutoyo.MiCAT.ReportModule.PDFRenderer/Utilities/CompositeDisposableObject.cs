// <copyright file="CompositeDisposableObject.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace Mitutoyo.MiCAT.ReportModule.PDFRenderer.Utilities
{
   public class CompositeDisposableObject : ICompositeDisposableObject
   {
      private List<IDisposable> components = new List<IDisposable>();

      public void Add(IDisposable component)
      {
         if (component != null)
         {
            this.components.Add(component);
         }
      }

      public void Dispose()
      {
         foreach (var component in this.components)
         {
            component.Dispose();
         }

         this.components.Clear();
      }

      public bool HasComponents()
      {
         return this.components.Any();
      }
   }
}
