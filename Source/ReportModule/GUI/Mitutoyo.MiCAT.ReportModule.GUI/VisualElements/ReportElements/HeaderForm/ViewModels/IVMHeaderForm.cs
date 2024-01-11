// <copyright file="IVMHeaderForm.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.HeaderForm.ViewModels
{
   public interface IVMHeaderForm
   {
      IHeaderFormFieldController HeaderFormFieldController { get; }

      IEnumerable<VMDynamicPropertyItem> DynamicPropertyItems { get; set; }
   }
}
