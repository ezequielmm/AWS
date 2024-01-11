// <copyright file="DataServiceStateDataLoaderTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Persistence;
using Moq;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Test
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class DataServiceStateDataLoaderTest
   {
      [Test]
      public void GetData_Test()
      {
         var charDetails = new List<string>() { "Details A", "Details B", "Details C" }.ToImmutableList();
         Mock<ICharacteristicDetailsProvider> detailsProvider = new Mock<ICharacteristicDetailsProvider>();
         detailsProvider.Setup(dp => dp.GetAllCharacteristicDetails()).Returns(charDetails);

         Mock<IPartPersistence> partPersistence = new Mock<IPartPersistence>();
         var partList = Enumerable.Repeat(new PartDescriptor(), 3).ToList();
         partList[0].Id = Guid.NewGuid();
         partList[1].Id = Guid.NewGuid();
         partList[2].Id = Guid.NewGuid();

         partPersistence.Setup(p => p.GetParts()).Returns(Task.FromResult(partList.AsEnumerable()));

         Mock<IPlanPersistence> planPersistence = new Mock<IPlanPersistence>();
         var planList = Enumerable.Repeat(new PlanDescriptor(), 5).ToList();
         planList[0].Part = new PartDescriptor() { Id = partList[1].Id };
         planList[1].Part = new PartDescriptor() { Id = partList[0].Id };
         planList[2].Part = new PartDescriptor() { Id = partList[2].Id };
         planList[3].Part = new PartDescriptor() { Id = partList[1].Id };

         planPersistence.Setup(p => p.GetPlans()).Returns(Task.FromResult(planList.AsEnumerable()));

         Mock<IDynamicPropertyPersistence> dynamicPropertyPersistence = new Mock<IDynamicPropertyPersistence>();
         var dynamicPropertyDescriptionList = Enumerable.Repeat(new DynamicPropertyDescriptor(), 3);
         dynamicPropertyPersistence.Setup(p => p.GetDynamicProperties()).Returns(Task.FromResult(dynamicPropertyDescriptionList));

         Mock<IMeasurementPersistence> measurementPersistence = new Mock<IMeasurementPersistence>();

         // Arrange
         DataServiceStateDataLoader sut = new DataServiceStateDataLoader(partPersistence.Object, planPersistence.Object, dynamicPropertyPersistence.Object, measurementPersistence.Object, detailsProvider.Object);

         // Act
         var data = sut.GetData();

         // Assert
         Assert.AreEqual(3, data.Result.DynamicProperties.Count());
         Assert.AreEqual(5, data.Result.Plans.Count());
         Assert.AreEqual(partList[1].Id, data.Result.Plans.ToList()[0].Part.Id);
         Assert.AreEqual(partList[0].Id, data.Result.Plans.ToList()[1].Part.Id);
         Assert.AreEqual(partList[2].Id, data.Result.Plans.ToList()[2].Part.Id);
         Assert.AreEqual(partList[1].Id, data.Result.Plans.ToList()[3].Part.Id);
         Assert.AreEqual(3, data.Result.AllCharacteristicDetails.Count());
         Assert.AreEqual("Details A", data.Result.AllCharacteristicDetails.ToList()[0]);
         Assert.AreEqual("Details B", data.Result.AllCharacteristicDetails.ToList()[1]);
         Assert.AreEqual("Details C", data.Result.AllCharacteristicDetails.ToList()[2]);
      }
   }
}
