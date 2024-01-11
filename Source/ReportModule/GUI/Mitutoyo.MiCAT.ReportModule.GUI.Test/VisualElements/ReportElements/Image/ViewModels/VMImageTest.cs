// <copyright file="VMImageTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements.Image.ViewModels;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements.Image.ViewModels
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class VMImageTest
   {
      private Mock<IImageController> _imageControllerMock;
      private Mock<IDeleteComponentController> _deleteComponentControllerMock;
      private Mock<IAppStateHistory> _history;
      private Mock<ISnapShot> _snapShot;
      private Mock<IActionCaller> _actionCallerMock;
      private ReportImage _reportImageEmpty;
      private ReportImage _reportImageWithImage;
      private ReportComponentPlacement _placementImageEmpty;
      private ReportComponentPlacement _placementImageWithImage;

      [SetUp]
      public void SetUp()
      {
         string image = "dfghfgdfgd667878nnvghv==";
         _imageControllerMock = new Mock<IImageController>();
         _deleteComponentControllerMock = new Mock<IDeleteComponentController>();
         _history = new Mock<IAppStateHistory>();
         _history.Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_history.Object);
         _snapShot = new Mock<ISnapShot>();
         _actionCallerMock = new Mock<IActionCaller>();
         _placementImageEmpty = new ReportComponentPlacement(new Id<ReportBody>(new UniqueValue()), 10, 0, 100, 10);
         _placementImageWithImage = new ReportComponentPlacement(new Id<ReportBody>(new UniqueValue()), 10, 0, 100, 10);
         _reportImageEmpty = new ReportImage(_placementImageEmpty);
         _reportImageWithImage = new ReportImage(_placementImageWithImage, image);
         var changeList = new Changes(new List<UpdateItemChange>() { new UpdateItemChange(_reportImageEmpty.Id), new UpdateItemChange(_reportImageWithImage.Id) });
         _snapShot.Setup(h => h.GetChanges()).Returns(changeList);
         _snapShot.Setup(h => h.GetItem(_reportImageEmpty.Id as IItemId<IReportComponent>)).Returns(_reportImageEmpty);
         _snapShot.Setup(h => h.GetItem(_reportImageWithImage.Id as IItemId<IReportComponent>)).Returns(_reportImageWithImage);
         _snapShot.Setup(s => s.GetItems<ReportModeState>()).Returns(new List<ReportModeState>() { new ReportModeState() });
         _history.Setup(h => h.CurrentSnapShot).Returns(_snapShot.Object);
      }

      [Test]
      public void VMImage_ShouldInitializeReportComponent()
      {
         var vmPlacement = new Mock<IVMReportComponentPlacement>();
         var sut = new VMImage(
            _history.Object,
            _reportImageWithImage.Id,
            vmPlacement.Object,
            _deleteComponentControllerMock.Object,
            _imageControllerMock.Object,
            _actionCallerMock.Object);

         Assert.AreEqual(sut.Image, Convert.FromBase64String(_reportImageWithImage.Image));
      }

      [Test]
      public void VMImage_ShouldInitializeImageWithoutContentWithDefaultImage()
      {
         var defaultImage = Convert.FromBase64String(@"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");
         var vmPlacement = new Mock<IVMReportComponentPlacement>();

         var sut = new VMImage(
                    _history.Object,
                    _reportImageEmpty.Id,
                    vmPlacement.Object,
                    _deleteComponentControllerMock.Object,
                    _imageControllerMock.Object,
                    _actionCallerMock.Object);

         Assert.AreEqual(sut.Image, defaultImage);
      }
   }
}