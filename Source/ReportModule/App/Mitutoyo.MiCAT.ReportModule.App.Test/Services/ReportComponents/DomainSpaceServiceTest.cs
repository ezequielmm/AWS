// <copyright file="DomainSpaceServiceTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.App.Test.Services.ReportComponents
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class DomainSpaceServiceTest
   {
      private Mock<ISnapShot> _snapShotMock;
      private DomainSpaceService _sut;

      [SetUp]
      public void SetUp()
      {
         _snapShotMock = new Mock<ISnapShot>();
         _sut = new DomainSpaceService();

         SetupSnapShotUpdateItem(_snapShotMock);
      }

      [Test]
      public void AddSpaceShouldMoveComponens()
      {
         //Arrange
         var reportBodyId = new Id<ReportBody>(Guid.NewGuid());

         _snapShotMock.Setup(s => s.GetItems<IReportComponent>()).Returns(GetReportComponents(reportBodyId));

         //Act
         var newSnapShot = _sut.AddSpace(_snapShotMock.Object, 35, 20, Array.Empty<Id<IReportComponent>>());

         //Assert
         var newReportComponents = newSnapShot.GetItems<IReportComponent>().ToList();

         Assert.AreEqual(6, newReportComponents.Count());
         AssertPlacement(newReportComponents[0].Placement, reportBodyId, 1, 10, 1, 15);
         AssertPlacement(newReportComponents[1].Placement, reportBodyId, 1, 20, 1, 25);
         AssertPlacement(newReportComponents[2].Placement, reportBodyId, 1, 30, 1, 35);
         AssertPlacement(newReportComponents[3].Placement, reportBodyId, 1, 60, 1, 45);
         AssertPlacement(newReportComponents[4].Placement, reportBodyId, 1, 70, 1, 55);
         AssertPlacement(newReportComponents[5].Placement, reportBodyId, 1, 80, 1, 65);
      }

      [Test]
      public void AddSpaceShouldMoveComponensExceptTheIndicatedOne()
      {
         //Arrange
         var reportBodyId = new Id<ReportBody>(Guid.NewGuid());
         var reportComponents = GetReportComponents(reportBodyId);
         _snapShotMock.Setup(s => s.GetItems<IReportComponent>()).Returns(reportComponents);

         //Act
         var newSnapShot = _sut.AddSpace(_snapShotMock.Object, 35, 20, new[] { reportComponents[4].Id });

         //Assert
         var newReportComponents = newSnapShot.GetItems<IReportComponent>().ToList();

         Assert.IsTrue(newReportComponents.Count() == 6);
         AssertPlacement(newReportComponents[0].Placement, reportBodyId, 1, 10, 1, 15);
         AssertPlacement(newReportComponents[1].Placement, reportBodyId, 1, 20, 1, 25);
         AssertPlacement(newReportComponents[2].Placement, reportBodyId, 1, 30, 1, 35);
         AssertPlacement(newReportComponents[3].Placement, reportBodyId, 1, 60, 1, 45);
         AssertPlacement(newReportComponents[4].Placement, reportBodyId, 1, 50, 1, 55);
         AssertPlacement(newReportComponents[5].Placement, reportBodyId, 1, 80, 1, 65);
      }

      [Test]
      public void AddSpaceShouldNotMoveAnyComponent()
      {
         //Arrange
         var reportBodyId = new Id<ReportBody>(Guid.NewGuid());
         var reportComponents = GetReportComponents(reportBodyId);
         _snapShotMock.Setup(s => s.GetItems<IReportComponent>()).Returns(reportComponents);

         //Act
         var newSnapShot = _sut.AddSpace(_snapShotMock.Object, 100, 150, Array.Empty<Id<IReportComponent>>());

         //Assert
         var newReportComponents = newSnapShot.GetItems<IReportComponent>().ToList();

         Assert.AreEqual(6, newReportComponents.Count());

         AssertPlacement(newReportComponents[0].Placement, reportBodyId, 1, 10, 1, 15);
         AssertPlacement(newReportComponents[1].Placement, reportBodyId, 1, 20, 1, 25);
         AssertPlacement(newReportComponents[2].Placement, reportBodyId, 1, 30, 1, 35);
         AssertPlacement(newReportComponents[3].Placement, reportBodyId, 1, 40, 1, 45);
         AssertPlacement(newReportComponents[4].Placement, reportBodyId, 1, 50, 1, 55);
         AssertPlacement(newReportComponents[5].Placement, reportBodyId, 1, 60, 1, 65);
      }

      private void SetupSnapShotUpdateItem(Mock<ISnapShot> snapShotMock)
      {
         ServicesTestHelper.SetupUpdateItem<IReportComponent>(snapShotMock);
      }

      private ReportComponentFake[] GetReportComponents(Id<ReportBody> reportBodyId)
      {
         return new[]
          {
            new ReportComponentFake(new ReportComponentPlacement(reportBodyId,1,10,1,15)),
            new ReportComponentFake(new ReportComponentPlacement(reportBodyId,1,20,1,25)),
            new ReportComponentFake(new ReportComponentPlacement(reportBodyId,1,30,1,35)),
            new ReportComponentFake(new ReportComponentPlacement(reportBodyId,1,40,1,45)),
            new ReportComponentFake(new ReportComponentPlacement(reportBodyId,1,50,1,55)),
            new ReportComponentFake(new ReportComponentPlacement(reportBodyId,1,60,1,65))
         };
      }

      private void AssertPlacement(ReportComponentPlacement actualPlacement, IItemId expectedReportSectionId, int expectedX, int expectedY, int expectedWidth, int expectedHeight)
      {
         Assert.AreEqual(expectedReportSectionId, actualPlacement.ReportSectionId);
         Assert.AreEqual(expectedX, actualPlacement.X);
         Assert.AreEqual(expectedY, actualPlacement.Y);
         Assert.AreEqual(expectedWidth, actualPlacement.Width);
         Assert.AreEqual(expectedHeight, actualPlacement.Height);
      }
   }
   public class ReportComponentFake : ReportComponentBase<ReportComponentFake>, IReportComponent
   {
      public ReportComponentFake(ReportComponentPlacement placement)
         : this(Guid.NewGuid(), placement)
      { }
      private ReportComponentFake(Id<ReportComponentFake> id, ReportComponentPlacement placement)
         : base(id, placement)
      { }

      public override ReportComponentFake WithPosition(int x, int y)
      {
         return new ReportComponentFake(Id, Placement.WithPosition(x, y));
      }

      public override ReportComponentFake WithSize(int widht, int height)
      {
         return new ReportComponentFake(Id, Placement.WithSize(widht, height));
      }
   }
}
