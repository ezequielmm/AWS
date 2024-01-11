// <copyright file="VisualChildrensByType.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Windows.Media;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Common.ComponentsPageViewRelation
{
   public class VisualChildrensByType<T> where T:Visual
   {
      public List<T> GetChildrensByType(Visual visualElement)
      {
         List<T> result = new List<T>();

         if (visualElement != null)
         {
            if (visualElement is T fe)
            {
               result.Add(fe);
            }
            else
            {
               for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visualElement); i++)
               {
                  var visual = VisualTreeHelper.GetChild(visualElement, i) as Visual;
                  result.AddRange(GetChildrensByType(visual));
               }
            }
         }

         return result;
      }
   }
}
