// <copyright file="PdfBackgroundNotifierFake.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Export.Fakes
{
   public class PdfBackgroundNotifierFake : IMessageNotifier
   {
      public PdfBackgroundNotifierFake()
      {
      }

      public void NotifyError(ResultException ex)
      {
         throw ex;
      }
   }
}
