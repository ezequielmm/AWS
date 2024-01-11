// <copyright file="VMReportComponentPlacementTests.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

//using System.Collections.Generic;
//using System.Collections.Immutable;
//using System.Diagnostics.CodeAnalysis;
//using System.Linq;
//using Mitutoyo.MiCAT.ApplicationState;
//using Mitutoyo.MiCAT.ApplicationState.Clients;
//using Mitutoyo.MiCAT.ApplicationState.MetaData;
//using Mitutoyo.MiCAT.ReportModule.App.AppState;
//using Mitutoyo.MiCAT.ReportModule.Domain.Components;
//using Mitutoyo.MiCAT.ReportModule.Domain.Data;
//using Mitutoyo.MiCAT.ReportModule.GUI.Common.LayoutCalculator;
//using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
//using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
//using Mitutoyo.MiCAT.ReportModuleApp.Utilities;
//using Moq;
//using NUnit.Framework;

//namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements
//{
//   [TestFixture]
//   [ExcludeFromCodeCoverage]
//   public class VMReportComponentPlacementTests
//   {
//      private IAppStateHistory _history;
//      private IReportComponentPlacementController _placementController;
//      private ISelectedComponentController _selectedComponentController;
//      private IDeleteComponentController _deleteComponentController;
//      private IRenderedData _renderedData;
//      private ISnapShot _snapShot;
//      private IActionCaller _actionCaller;
//      private IService _service;

//      [SetUp]
//      public void SetUp()
//      {
//         _snapShot = Mock.Of<ISnapShot>();
//         _history = Mock.Of<IAppStateHistory>();
//         Mock.Get(_history).Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_history);
//         Mock.Get(_history).Setup(h => h.CurrentSnapShot).Returns(_snapShot);
//         _placementController = Mock.Of<IReportComponentPlacementController>();
//         _selectedComponentController = Mock.Of<ISelectedComponentController>();
//         _deleteComponentController = Mock.Of<IDeleteComponentController>();
//         _renderedData = Mock.Of<IRenderedData>();
//         _actionCaller = Mock.Of<IActionCaller>();
//         _service = Mock.Of<IService>();

//         Mock
//            .Get(_snapShot)
//            .Setup(x => x.GetItems<ReportModeState>())
//            .Returns(new[] { new ReportModeState(), });
//      }

//      [Test]
//      public void Constructor_ShouldCall_InitializeValuesFromSnapShot()
//      {
//         Element.Service = _service;

//         var element = new Element(
//            _history,
//            Mock.Of<IItemId>(),
//            Mock.Of<IVMReportComponentPlacement>(),
//            _deleteComponentController);

//         Mock
//            .Get(_service)
//            .Verify(x => x.InitializeFromSnapShot(_snapShot), Times.Once);
//      }

//      [Test]
//      public void Constructor_ShouldCall_UpdateFromBusinessEntity_WhenComponentIsNotNull()
//      {
//         Mock
//            .Get(_service)
//            .Setup(x => x.GetReportComponentEntity())
//            .Returns(Mock.Of<IReportComponent>());

//         Element.Service = _service;

//         var element = new Element(
//            _history,
//            Mock.Of<IItemId>(),
//            Mock.Of<IVMReportComponentPlacement>(),
//            _deleteComponentController);

//         Mock
//            .Get(_service)
//            .Verify(x => x.UpdateFromBusinessEntity(), Times.Once);
//      }

//      [Test]
//      public void Constructor_ShouldNotCall_UpdateFromBusinessEntity_WhenComponentIsNull()
//      {
//         Mock
//            .Get(_service)
//            .Setup(x => x.GetReportComponentEntity())
//            .Returns((IReportComponent)null);

//         Element.Service = _service;

//         var element = new Element(
//            _history,
//            Mock.Of<IItemId>(),
//            Mock.Of<IVMReportComponentPlacement>(),
//            _deleteComponentController);

//         Mock
//            .Get(_service)
//            .Verify(x => x.UpdateFromBusinessEntity(), Times.Never);
//      }

//      [Test]
//      [TestCase(1, 2, 3, 4)]
//      [TestCase(10, 20, 30, 40)]
//      public void UpdateFromBusinessEntity_ShouldSetDomainValues(int xPos, int yPos, int width, int height)
//      {
//         Mock
//            .Get(_service)
//            .Setup(x => x.GetReportComponentEntity())
//            .Returns(
//               new Component
//               {
//                  X = xPos,
//                  Y = yPos,
//                  Width = width,
//                  Height = height
//               });

//         Element.Service = _service;

//         var element = new Element(
//            _history,
//            _placementController,
//            _selectedComponentController,
//            Mock.Of<IItemId>(),
//            _renderedData,
//            _snapShot,
//            _actionCaller,
//            _deleteComponentController);

//         Assert.IsNotNull(element);
//         Assert.AreEqual(xPos, element.DomainX);
//         Assert.AreEqual(yPos, element.DomainY);
//         Assert.AreEqual(width, element.DomainWidth);
//         Assert.AreEqual(height, element.DomainHeight);
//      }

//      [Test]
//      public void UpdateFromBusinessEntity_ShouldRaiseReportComponentMoved_WhenDomainYHasChanged()
//      {
//         Mock
//            .Get(_service)
//            .Setup(x => x.GetReportComponentEntity())
//            .Returns(new Component());

//         Element.Service = _service;

//         var element = new Element(
//            _history,
//            _placementController,
//            _selectedComponentController,
//            Mock.Of<IItemId>(),
//            _renderedData,
//            _snapShot,
//            _actionCaller,
//            _deleteComponentController);

//         Mock
//           .Get(_service)
//           .Setup(x => x.GetReportComponentEntity())
//           .Returns(
//              new Component
//              {
//                 Y = 100,
//              });

//         element.Update(_snapShot);

//         Mock
//            .Get(_service)
//            .Verify(x => x.ReportComponentMoved(), Times.Once);
//      }

//      [Test]
//      public void UpdateFromBusinessEntity_ShouldNotRaiseReportComponentMoved_WhenDomainYHasNotChanged()
//      {
//         Mock
//            .Get(_service)
//            .Setup(x => x.GetReportComponentEntity())
//            .Returns(new Component());

//         Element.Service = _service;

//         var element = new Element(
//            _history,
//            _placementController,
//            _selectedComponentController,
//            Mock.Of<IItemId>(),
//            _renderedData,
//            _snapShot,
//            _actionCaller,
//            _deleteComponentController);

//         element.Update(_snapShot);

//         Mock
//            .Get(_service)
//            .Verify(x => x.ReportComponentMoved(), Times.Never);
//      }

//      [Test]
//      public void UpdateFromBusinessEntity_ShouldRaiseReportComponentLayoutChanged_WhenDomainHeightHasChanged()
//      {
//         Mock
//            .Get(_service)
//            .Setup(x => x.GetReportComponentEntity())
//            .Returns(new Component());

//         Element.Service = _service;

//         var element = new Element(
//            _history,
//            _placementController,
//            _selectedComponentController,
//            Mock.Of<IItemId>(),
//            _renderedData,
//            _snapShot,
//            _actionCaller,
//            _deleteComponentController);

//         Mock
//           .Get(_service)
//           .Setup(x => x.GetReportComponentEntity())
//           .Returns(
//              new Component
//              {
//                 Height = 100,
//              });

//         element.Update(_snapShot);

//         Mock
//            .Get(_service)
//            .Verify(x => x.ReportComponentMoved(), Times.Never);

//         Mock
//            .Get(_service)
//            .Verify(x => x.ReportComponentLayoutChanged(), Times.Once);
//      }

//      [Test]
//      public void UpdateFromBusinessEntity_ShouldNotRaiseReportComponentLayoutChanged_WhenDomainHeightHasNotChanged()
//      {
//         Mock
//            .Get(_service)
//            .Setup(x => x.GetReportComponentEntity())
//            .Returns(new Component());

//         Element.Service = _service;

//         var element = new Element(
//            _history,
//            Mock.Of<Id<ReportComponentPlacement>>(),
//            _placementController,
//            _selectedComponentController,
//            _renderedData);

//         element.Update(_snapShot);

//         Mock
//            .Get(_service)
//            .Verify(x => x.ReportComponentMoved(), Times.Never);

//         Mock
//            .Get(_service)
//            .Verify(x => x.ReportComponentLayoutChanged(), Times.Never);
//      }

//      public interface IService
//      {
//         void InitializeFromSnapShot(ISnapShot snapShot);
//         void UpdateFromPlacementEntity();
//         void ReportComponentMoved();
//         void ReportComponentLayoutChanged();
//      }

//      public class Component : IReportComponent
//      {
//         public bool HasForeignIds { get; set; }
//         public IItemId Id { get; set; }
//         public Id<ReportComponentPlacement> PlacementId { get; set; }

//         public ReportComponentPlacement Placement => throw new System.NotImplementedException();

//         public TypeMeta TypeMeta => throw new System.NotImplementedException();

//         public ImmutableArray<IPropertyMeta> PropertyMetas => throw new System.NotImplementedException();

//         public IEnumerable<object> PropertyValues => throw new System.NotImplementedException();

//         public IEnumerable<IItemId> ForeignIds()
//         {
//            return Enumerable.Empty<IItemId>();
//         }

//         public IReportComponent WithPosition(int x, int y)
//         {
//            throw new System.NotImplementedException();
//         }

//         public IReportComponent WithSize(int widht, int height)
//         {
//            throw new System.NotImplementedException();
//         }
//      }

//      public class Element : VMReportBodyPlacement
//      {
//         public static IService Service { get; set; }

//         public Element(
//            IAppStateHistory history,
//            Id<ReportComponentPlacement> id,
//            IPlacementController placementController,
//            ISelectedComponentController selectedComponentController,
//            IRenderedData renderData) :
//            base(
//               id,
//               history,
//               placementController,
//               selectedComponentController,
//               renderData)
//         {
//            ReportComponentMoved += (source, args) => Service.ReportComponentMoved();
//            ReportComponentLayoutChanged += (source, args) => Service.ReportComponentLayoutChanged();
//         }

//         protected override void InitializeFromSnapShot(ISnapShot snapShot)
//         {
//            base.InitializeFromSnapShot(snapShot);
//            Service?.InitializeFromSnapShot(snapShot);
//         }
//         protected override void UpdateFromPlacementEntity(ReportComponentPlacement placementEntity)
//         {
//            base.UpdateFromPlacementEntity(placementEntity);
//            Service?.UpdateFromPlacementEntity();
//         }
//      }
//   }
//}
