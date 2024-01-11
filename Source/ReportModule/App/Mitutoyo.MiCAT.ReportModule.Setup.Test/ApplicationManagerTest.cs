// <copyright file="ApplicationManagerTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Mitutoyo.MiCAT.ReportModule.Domain;
using Mitutoyo.MiCAT.ReportModule.GUI;
using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.Zoom;
using NUnit.Framework;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ApplicationManagerTest
   {
      [Test]
      public void Initialize_ShouldRegisterClasses()
      {
         // arrange
         InitializeApp();

         var applicationManager = new ApplicationManager();
         var iocContainer = applicationManager.IOCContainer;

         // act
         applicationManager.Start();

         // assert
         CheckResolution<VMReportViewWorkspace>(iocContainer);
         CheckResolution<VMZoomFactor>(iocContainer);
         CheckResolution<VMPageNumberInfo>(iocContainer);
         CheckResolution<IPageSizeList>(iocContainer);
         CheckResolution<IRenderedData>(iocContainer);
      }

      private void InitializeApp()
      {
         //Initialize an App to have a Application.Current
         var waitForApplicationRun = new TaskCompletionSource<bool>();
         var appThread = new Thread(
            () =>
            {
               var application = new Application();
               application.Startup += (s, e) => { waitForApplicationRun.SetResult(true); };
               application.Run();
            }
         );

         appThread.SetApartmentState(ApartmentState.STA);
         appThread.IsBackground = true;
         appThread.Start();

         waitForApplicationRun.Task.Wait();
      }

      private void CheckResolution<TTypeToResolve>(IUnityContainer iocContainer)
      {
         Assert.IsNotNull(iocContainer.Resolve<TTypeToResolve>());
      }
   }
}
