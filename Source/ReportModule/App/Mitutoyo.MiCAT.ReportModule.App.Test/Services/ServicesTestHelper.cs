// <copyright file="ServicesTestHelper.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModuleApp.Services.ReportComponents;
using Moq;

namespace Mitutoyo.MiCAT.ReportModule.App.Test.Services
{
   public static class ServicesTestHelper
   {
      public static void SetupUpdateItem<T>(Mock<ISnapShot> snapShotMock) where T : IStateItem
      {
         snapShotMock.Setup(ss => ss.UpdateItem(It.IsAny<T>(), It.IsAny<T>()))
            .Returns(new Func<T, T, ISnapShot>((oldItem, newItem) =>
            {
               SetupGetItemsForNewItem(snapShotMock, oldItem, newItem);
               return snapShotMock.Object;
            }));
      }

      public static void SetupGetItemsForNewItem<T>(Mock<ISnapShot> snapShotMock, T newItem) where T : IStateItem
      {
         var currentItems = snapShotMock.Object.GetItems<T>().ToImmutableList();
         ImmutableList<T> updatedItems;

         if (currentItems.Contains(newItem))
         {
            var oldItem = currentItems.Single(item => item.Id == newItem.Id);
            updatedItems = currentItems.Replace(oldItem, newItem);
         }
         else
         {
            updatedItems = ImmutableList<T>.Empty.Add(newItem);
         }

         SetupGetItemsForNewItem(snapShotMock, updatedItems, newItem);
      }

      public static void SetupAddSpace(this Mock<IDomainSpaceService> domainSpaceServiceMock, ISnapShot snapShotResult)
      {
         domainSpaceServiceMock.Setup(d => d.AddSpace(It.IsAny<ISnapShot>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<IItemId<IReportComponent>>>()))
            .Returns(snapShotResult);
      }

      public static void VerifyCallAddSpace(this Mock<IDomainSpaceService> domainSpaceServiceMock, IItemId<IReportComponent> expectedPlacementId, int expectedYFakeSpaceBegin, int expectedHeightFakeSpace)
      {
         domainSpaceServiceMock.Verify(d => d.AddSpace(
            It.IsAny<ISnapShot>(),
            It.Is<int>(y => y == expectedYFakeSpaceBegin),
            It.Is<int>(s => s == expectedHeightFakeSpace),
            It.Is<IEnumerable<IItemId<IReportComponent>>>(ids => ids.Single().UniqueValue == expectedPlacementId.UniqueValue)),
            Times.Once());
      }

      public static void VerifyCallAddSpace(this Mock<IDomainSpaceService> domainSpaceServiceMock, int expectedYFakeSpaceBegin, int expectedHeightFakeSpace)
      {
         domainSpaceServiceMock.Verify(d => d.AddSpace(
            It.IsAny<ISnapShot>(),
            It.Is<int>(y => y == expectedYFakeSpaceBegin),
            It.Is<int>(s => s == expectedHeightFakeSpace),
            It.Is<IEnumerable<IItemId<IReportComponent>>>(ids => !ids.Any())),
            Times.Once());
      }

      private static void SetupGetItemsForNewItem<T>(Mock<ISnapShot> snapShotMock, T oldItem, T newItem) where T : IStateItem
      {
         var currentItems = snapShotMock.Object.GetItems<T>().ToImmutableList();
         var updatedItems = currentItems.Replace(oldItem, newItem);

         SetupGetItemsForNewItem(snapShotMock, updatedItems, newItem);
      }

      private static void SetupGetItemsForNewItem<T>(Mock<ISnapShot> snapShotMock, IEnumerable<T> updatedItems, T newItem) where T : IStateItem
      {
         snapShotMock.Setup(ss => ss.GetItems<T>()).Returns(updatedItems);
         snapShotMock.Setup(s => s.GetItem(It.Is<IItemId<T>>(id => id.UniqueValue == newItem.Id.UniqueValue))).Returns(newItem);
         snapShotMock.Setup(s => s.GetItem<T>(It.Is<IItemId>(id => id.UniqueValue == newItem.Id.UniqueValue))).Returns(newItem);
      }
   }
}
