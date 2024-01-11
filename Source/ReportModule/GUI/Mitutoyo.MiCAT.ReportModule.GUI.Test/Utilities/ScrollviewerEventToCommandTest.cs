// <copyright file="ScrollviewerEventToCommandTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using Mitutoyo.MiCAT.ReportModule.GUI.MainWindow.Zoom;
using Mitutoyo.MiCAT.ReportModule.GUI.Utilities;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.Utilities
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class ScrollviewerEventToCommandTest
   {
      private ScrollviewerEventToCommandWrapToTest _eventToCommand;
      private Mock<ScrollViewer> _ScrollViewerMock;
      private Mock<IVMZoomFactor> _vmZoomFactorMock;
      private Mock<ICommand> _commandMock;

      [Apartment(ApartmentState.STA)]
      [SetUp]
      public void Setup()
      {
         _ScrollViewerMock = new Mock<ScrollViewer>();
         _vmZoomFactorMock = new Mock<IVMZoomFactor>();
         _commandMock = new Mock<ICommand>();
         _vmZoomFactorMock.Setup(p => p.ZoomCommand).Returns(_commandMock.Object);
         _eventToCommand = new ScrollviewerEventToCommandWrapToTest(_ScrollViewerMock.Object);
         _eventToCommand.Command = _vmZoomFactorMock.Object.ZoomCommand;
      }
      [Apartment(ApartmentState.STA)]
      [Test]
      public void InvokeWheelWithoutKeyShouldDoNothing()
      {
         var mouseWheelArgs = new MouseWheelEventArgs(Mouse.PrimaryDevice, 0, 1);
         _eventToCommand.Execute(mouseWheelArgs);
         _vmZoomFactorMock.Verify(v=>v.Scale,Times.Never);
      }
      [Apartment(ApartmentState.STA)]
      [Test]
      public void InvokeKeyWithoutKeyShouldDoNothing()
      {
         var keyArgs = new KeyboardEventArgs(Keyboard.PrimaryDevice, 0);
         _eventToCommand.Execute(keyArgs);
         _vmZoomFactorMock.Verify(v => v.Scale, Times.Never);
      }
      [Apartment(ApartmentState.STA)]
      [Test]
      public void GetCommandSet()
      {
         var command = _eventToCommand.Command;
         Assert.NotNull(command);
      }
   }
   [ExcludeFromCodeCoverage]
   public class ScrollviewerEventToCommandWrapToTest : ScrollviewerEventToCommand
   {
      public ScrollviewerEventToCommandWrapToTest(ScrollViewer scrollViewer)
      {
         this.Attach(scrollViewer);
      }
      public void Execute(object parameters)
      {
         Invoke(parameters);
      }
   }
}
