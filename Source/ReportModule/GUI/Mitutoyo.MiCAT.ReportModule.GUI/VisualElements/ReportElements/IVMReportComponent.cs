// <copyright file="IVMReportComponent.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.ComponentModel;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ApplicationState.Clients;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;

namespace Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements
{
   public interface IVMReportComponent : IVMVisualElement, INotifyPropertyChanged, IUpdateClient, IDisposable
   {
      new IVMReportComponentPlacement VMPlacement { get; }
      IItemId<IReportComponent> Id { get; }
   }
}
