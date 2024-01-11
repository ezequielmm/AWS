// <copyright file="VMReportComponentTests.cs" company="Mitutoyo Europe GmbH">
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
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements;
using Mitutoyo.MiCAT.ReportModule.GUI.VisualElements.ReportElements;
using Mitutoyo.MiCAT.ReportModuleApp.Controllers;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.GUI.Test.VisualElements.ReportElements
{
   [TestFixture]
   [ExcludeFromCodeCoverage]
   public class VMReportComponentTests
   {
      private IAppStateHistory _history;
      private IVMReportComponentPlacement _vmReportComponentPlacement;
      private IDeleteComponentController _deleteComponentController;
      private ISnapShot _snapShot;
      private IService _service;
      private IReportComponent _entity;
      private IItemId<IReportComponent> _entityId;

      [SetUp]
      public void SetUp()
      {
         _entityId = new Id<IReportComponent>(Guid.NewGuid());
         _entity = Mock.Of<IReportComponent>();

         _vmReportComponentPlacement = Mock.Of<IVMReportComponentPlacement>();
         _snapShot = Mock.Of<ISnapShot>();
         _history = Mock.Of<IAppStateHistory>();

         Mock.Get(_history).Setup(h => h.AddClient(It.IsAny<ISubscriptionClient>(), 0)).Returns(_history);
         Mock.Get(_history).Setup(h => h.CurrentSnapShot).Returns(_snapShot);
         _deleteComponentController = Mock.Of<IDeleteComponentController>();
         _service = Mock.Of<IService>();
         Mock.Get(_entity).Setup(e => e.Id).Returns(_entityId);
         Mock.Get(_entity).Setup(e => e.Placement).Returns(new ReportComponentPlacement(new Id<ReportBody>(Guid.NewGuid()), 0, 0, 0, 0));
         Mock.Get(_snapShot).Setup(s => s.GetItem(_entityId)).Returns(_entity);

         Mock
            .Get(_snapShot)
            .Setup(x => x.GetItems<ReportModeState>())
            .Returns(new[] { new ReportModeState(), });

         Mock
            .Get(_snapShot)
            .Setup(x => x.GetItems<ReportComponentSelection>())
            .Returns(new[] { new ReportComponentSelection(), });
      }

      [Test]
      public void VMReportComponent_ShouldInitializeRenderMode_WhenReportModeStateIsOnEditMode()
      {
         // Arrange
         Mock
            .Get(_snapShot)
            .Setup(x => x.GetItems<ReportModeState>())
            .Returns(new[] { new ReportModeState(true), });

         // Arrange
         var vmToTest = new VMReportComponent(
            _history,
            _entityId,
            _vmReportComponentPlacement,
            _deleteComponentController);

         // Assert
         Assert.AreEqual(RenderMode.EditMode, vmToTest.RenderMode);
      }

      [Test]
      public void VMReportComponent_ShouldInitializeRenderMode_WhenReportModeStateIsOnViewMode()
      {
         // Arrange
         Mock
            .Get(_snapShot)
            .Setup(x => x.GetItems<ReportModeState>())
            .Returns(new[] { new ReportModeState(false), });

         // Act
         var vmToTest = new VMReportComponent(
            _history,
            _entityId,
            _vmReportComponentPlacement,
            _deleteComponentController);

         // Assert
         Assert.AreEqual(RenderMode.ViewMode, vmToTest.RenderMode);
         Assert.AreEqual(DisplayMode.Normal, vmToTest.DisplayMode);
      }

      [Test]
      public void Constructor_ShouldCall_InitializeValuesFromSnapShot()
      {
         Element.Service = _service;

         var element = new Element(
            _history,
            _entityId,
            _vmReportComponentPlacement,
            _deleteComponentController);

         Mock.Get(_service).Verify(x => x.InitializeFromSnapShot(_snapShot), Times.Once);
         Mock.Get(_vmReportComponentPlacement).Verify(vm => vm.UpdateFromPlacementEntity(null, _entity.Placement), Times.Once);
      }

      [Test]
      public void ShouldCall_UpdateFromBusinessEntity_WhenEntityHasChanged()
      {
         // Arrange
         var newSnapShot = Mock.Of<ISnapShot>();
         var newEntity = Mock.Of<IReportComponent>();
         var entityChanges = new Changes(new List<IItemChange>() { new UpdateItemChange(_entityId, _entity, newEntity) });

         Element.Service = _service;
         Mock.Get(newEntity).Setup(e => e.Id).Returns(_entityId);
         Mock.Get(newEntity).Setup(e => e.Placement).Returns(new ReportComponentPlacement(new Id<ReportBody>(Guid.NewGuid()), 0, 0, 0, 0));

         Mock.Get(newSnapShot).Setup(s => s.GetChanges()).Returns(entityChanges);
         Mock.Get(newSnapShot).Setup(x => x.GetItems<ReportModeState>()).Returns(new[] { new ReportModeState(), });

         var element = new Element(
            _history,
            _entityId,
            _vmReportComponentPlacement,
            _deleteComponentController);

         // Act
         element.Update(newSnapShot);

         // Assert
         Mock.Get(_service).Verify(x => x.UpdateFromBusinessEntity(null, _entity), Times.Once);
         Mock.Get(_service).Verify(x => x.UpdateFromBusinessEntity(_entity, newEntity), Times.Once);
         Mock.Get(_vmReportComponentPlacement).Verify(vm => vm.UpdateFromPlacementEntity(_entity.Placement, newEntity.Placement), Times.Once);
         Mock.Get(_vmReportComponentPlacement).Verify(vm => vm.UpdateFromPlacementEntity(null, _entity.Placement), Times.Once);
      }

      [Test]
      public void ShouldNotCall_UpdateFromBusinessEntity_WhenEntityHasBeenDeleted()
      {
         // Arrange
         var newSnapShot = Mock.Of<ISnapShot>();
         var entityChanges = new Changes(new List<IItemChange>() { new DeleteItemChange(_entityId, _entity) });

         Element.Service = _service;

         Mock.Get(newSnapShot).Setup(s => s.GetChanges()).Returns(entityChanges);
         Mock.Get(newSnapShot).Setup(x => x.GetItems<ReportModeState>()).Returns(new[] { new ReportModeState(), });

         var element = new Element(
            _history,
            _entityId,
            _vmReportComponentPlacement,
            _deleteComponentController);

         // Act
         element.Update(newSnapShot);

         // Assert
         Mock.Get(_service).Verify(x => x.UpdateFromBusinessEntity(null, _entity), Times.Once);
         Mock.Get(_vmReportComponentPlacement).Verify(vm => vm.UpdateFromPlacementEntity(null, _entity.Placement), Times.Once);
         Mock.Get(_service).Verify(x => x.UpdateFromBusinessEntity(It.Is<IReportComponent>(r => r != null), It.IsAny<IReportComponent>()), Times.Never);
         Mock.Get(_vmReportComponentPlacement).Verify(vm => vm.UpdateFromPlacementEntity(It.Is<ReportComponentPlacement>(r => r != null), It.IsAny<ReportComponentPlacement>()), Times.Never);
      }

      [Test]
      public void ShouldNotCall_UpdateFromBusinessEntity_WhenEntityHasntChanged()
      {
         // Arrange
         var newSnapShot = Mock.Of<ISnapShot>();
         var entityChanges = new Changes(new List<IItemChange>());

         Element.Service = _service;

         Mock.Get(newSnapShot).Setup(x => x.GetItems<ReportModeState>()).Returns(new[] { new ReportModeState(), });
         Mock.Get(newSnapShot).Setup(s => s.GetChanges()).Returns(entityChanges);

         var element = new Element(
            _history,
            _entityId,
            _vmReportComponentPlacement,
            _deleteComponentController);

         // Act
         element.Update(newSnapShot);

         // Assert
         Mock.Get(_service).Verify(x => x.UpdateFromBusinessEntity(null, _entity), Times.Once);
         Mock.Get(_vmReportComponentPlacement).Verify(vm => vm.UpdateFromPlacementEntity(null, _entity.Placement), Times.Once);
         Mock.Get(_service).Verify(x => x.UpdateFromBusinessEntity(It.Is<IReportComponent>(r => r != null), It.IsAny<IReportComponent>()), Times.Never);
         Mock.Get(_vmReportComponentPlacement).Verify(vm => vm.UpdateFromPlacementEntity(It.Is<ReportComponentPlacement>(r => r != null), It.IsAny<ReportComponentPlacement>()), Times.Never);
      }

      public interface IService
      {
         void InitializeFromSnapShot(ISnapShot snapShot);
         void UpdateFromBusinessEntity(IReportComponent reportComponentBefore, IReportComponent reportComponentAfter);
      }

      public class Element : VMReportComponent
      {
         public static IService Service { get; set; }

         public Element(
            IAppStateHistory history,
            IItemId<IReportComponent> id,
            IVMReportComponentPlacement vmPlacement,
            IDeleteComponentController deleteComponentController) :
            base(
               history,
               id,
               vmPlacement,
               deleteComponentController)
         {
         }

         protected override void InitializeFromSnapShot(ISnapShot snapShot)
         {
            base.InitializeFromSnapShot(snapShot);
            Service?.InitializeFromSnapShot(snapShot);
         }

         protected override void UpdateFromBusinessEntity(IReportComponent reportComponentBefore, IReportComponent reportComponentAfter)
         {
            base.UpdateFromBusinessEntity(reportComponentBefore, reportComponentAfter);
            Service?.UpdateFromBusinessEntity(reportComponentBefore, reportComponentAfter);
         }
      }
   }
}
