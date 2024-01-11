// <copyright file="IAsyncCommand.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Threading.Tasks;
using System.Windows.Input;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Utilities
{
   public interface IAsyncCommand : ICommand
   {
      Task ExecuteAsync(object parameter);
   }
}
