﻿// <copyright file="IVMSelectablePlacement.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements
{
   public interface IVMSelectablePlacement : IVMVisualPlacement
   {
      bool IsSelectable { get; }
      bool IsSelected { get; }

      void Select(object obj);
      RelayCommand<object> SelectCommand { get; }
   }
}
