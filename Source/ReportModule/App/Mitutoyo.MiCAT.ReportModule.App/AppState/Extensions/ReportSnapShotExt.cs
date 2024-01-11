// <copyright file="ReportSnapShotExt.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Mitutoyo.MiCAT.ApplicationState;
using Mitutoyo.MiCAT.ReportModule.App.AppState;
using Mitutoyo.MiCAT.ReportModule.Domain.Components;
using Mitutoyo.MiCAT.ReportModule.Domain.Components.ReportSections;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.Visibility;

namespace Mitutoyo.MiCAT.ReportModuleApp.AppState.Extensions
{
   public static class ReportSnapShotExt
   {
      public static ReportBody GetReportBody(this ISnapShot snapShot)
      {
         return snapShot.GetItems<ReportBody>().Single();
      }
      public static IItemId GetReportBodyId(this ISnapShot snapShot)
      {
         return snapShot.GetReportBody().Id;
      }
      public static ReportHeader GetReportHeader(this ISnapShot snapShot)
      {
         return snapShot.GetItems<ReportHeader>().Single();
      }
      public static IItemId GetReportHeaderId(this ISnapShot snapShot)
      {
         return snapShot.GetReportHeader().Id;
      }
      public static ReportFooter GetReportFooter(this ISnapShot snapShot)
      {
         return snapShot.GetItems<ReportFooter>().Single();
      }
      public static IItemId GetReportFooterId(this ISnapShot snapShot)
      {
         return snapShot.GetReportFooter().Id;
      }

      public static IEnumerable<IItemId<IReportComponent>> GetSelectedReportComponentIds(this ISnapShot snapShot)
      {
         return snapShot.GetItems<ReportComponentSelection>().Single().SelectedReportComponentIds;
      }

      public static bool IsComponentSelected(this ISnapShot snapShot, IItemId<IReportComponent> reportComponentId)
      {
         return GetSelectedReportComponentIds(snapShot).Contains(reportComponentId);
      }

      public static bool IsReportInEditMode(this ISnapShot snapShot)
      {
         var reportModeState = snapShot.GetItems<ReportModeState>().Single();
         return (reportModeState.EditMode == true);
      }

      public static ReportSectionSelection GetReportSectionSelection(this ISnapShot snapShot)
      {
         return snapShot.GetItems<ReportSectionSelection>().Single();
      }
      public static bool IsSectionSelected (this ISnapShot snapShot, IItemId sectionId)
      {
         return GetReportSectionSelection(snapShot).SelectedSectionIds.Contains(sectionId);
      }

      public static bool IsReportBodySectionSelected(this ISnapShot snapShot)
      {
         return IsSectionSelected(snapShot, snapShot.GetReportBodyId());
      }

      public static CommonPageLayout GetCommonPageLayout(this ISnapShot snapShot)
      {
         return snapShot.GetItems<CommonPageLayout>().SingleOrDefault();
      }

      public static IEnumerable<string> GetCharacteristicTypes(this ISnapShot snapShot)
      {
         var measurementResultState = snapShot.GetItems<AllCharacteristicTypes>().Single();
         return measurementResultState.CharacteristicTypes;
      }

      public static IEnumerable<string> GetCharacteristicDetails(this ISnapShot snapShot)
      {
         var detailsState = snapShot.GetItems<AllCharacteristicDetails>().Single();
         return detailsState.Details;
      }

      public static IEnumerable<IReportComponent> GetAllReportComponents(this ISnapShot snapShot)
      {
         return snapShot.GetItems<IReportComponent>();
      }
      public static IEnumerable<IReportComponent> GetAllReportComponentsOnBody(this ISnapShot snapShot)
      {
         return snapShot.GetAllReportComponents().Where(c => c.Placement.ReportSectionId == snapShot.GetReportBodyId());
      }
      public static IEnumerable<IItemId<IReportComponent>> GetAllReportComponentIdsOnHeader(this ISnapShot snapShot)
      {
         return snapShot.GetBackReferenceIds(snapShot.GetReportHeaderId()).OfType<IItemId<IReportComponent>>();
      }

      public static IEnumerable<IItemId<IReportComponent>> GetAllReportComponentIdsOnFooter(this ISnapShot snapShot)
      {
         return snapShot.GetBackReferenceIds(snapShot.GetReportFooterId()).OfType<IItemId<IReportComponent>>();
      }

      public static IEnumerable<IStateItem> GetAllChildReportComponentItemsOnBody(this ISnapShot snapShot)
      {
         return snapShot
            .GetAllReportComponentsOnBody()
            .SelectMany(x => snapShot.GetReportComponentDataItems(x));
      }

      public static bool HasBeenDeleted(this ISnapShot snapShot, IItemId itemId)
      {
         return snapShot.GetChanges()
            .Any(x => x.ItemId.Equals(itemId) && x is DeleteItemChange);
      }

      public static bool HasBeenUpdated<T>(this ISnapShot snapShot)
      {
         return snapShot.GetChanges()
            .OfType<UpdateItemChange>()
            .Any(x => x.ItemType == typeof(T));
      }

      public static bool HasBeenAdded<T>(this ISnapShot snapShot)
      {
         return snapShot.GetChanges()
            .OfType<AddItemChange>()
            .Any(x => x.ItemType == typeof(T));
      }

      public static bool HasBeenAddedOrUpdated<T>(this ISnapShot snapShot)
      {
         return snapShot.HasBeenAdded<T>() || snapShot.HasBeenUpdated<T>();
      }

      public static IEnumerable<IItemId<IReportComponent>> GetReportComponentIdsAddedOnContainer(this ISnapShot snapShot, IItemId containerId)
      {
         return snapShot.GetChanges().OfType<AddItemChange>()
            .Where(rc => rc.AfterItem.ForeignIds().Contains(containerId))
            .Select(x => x.ItemId).Cast<IItemId<IReportComponent>>();
      }
      public static IEnumerable<IItemId<IReportComponent>> GetAllReportComponentIdsAddedOnBody(this ISnapShot snapShot)
      {
         return snapShot.GetReportComponentIdsAddedOnContainer(snapShot.GetReportBodyId());
      }

      public static IEnumerable<IItemId<IReportComponent>> GetReportComponentIdsDeletedOnContainer(this ISnapShot snapShot, IItemId containerId)
      {
         return snapShot.GetChanges().OfType<DeleteItemChange>()
            .Where(rc => rc.BeforeItem.ForeignIds().Contains(containerId))
            .Select(x => x.ItemId).OfType<IItemId<IReportComponent>>();
      }
      public static IEnumerable<IItemId<IReportComponent>> GetAllReportComponentIdsDeletedOnBody(this ISnapShot snapShot)
      {
         return snapShot.GetReportComponentIdsDeletedOnContainer(snapShot.GetReportBodyId());
      }

      public static IImmutableList<DynamicPropertyValue> GetCurrentDynamicPropertyValues(this ISnapShot snapShot)
      {
         var selectedRunData = snapShot.GetItems<RunSelection>().Single().SelectedRunData;

         if (selectedRunData == null)
            return ImmutableList<DynamicPropertyValue>.Empty;
         else
            return selectedRunData.DynamicPropertyValues;
      }

      public static bool IsViewVisible(this ISnapShot snapShot, ViewElement viewElement)
      {
         return snapShot.GetItems<ViewVisibility>().Single(v => v.ViewElement == viewElement).IsVisible;
      }

      public static ViewVisibility GetViewVisibility(this ISnapShot snapShot, ViewElement viewElement)
      {
         return snapShot.GetItems<ViewVisibility>().Single(v => v.ViewElement == viewElement);
      }

      private static IEnumerable<IStateItem> GetReportComponentDataItems(this ISnapShot snapShot, IStateItem stateItem)
      {
         return stateItem is IReportComponentDataItemsContainer itemContainer ?
            itemContainer
               .ReportComponentDataItemIds
               .SelectMany(x =>
               {
                  var extraItem = snapShot.GetItem(x);

                  return snapShot
                     .GetReportComponentDataItems(extraItem)
                     .Append(extraItem);
               }) :
            Enumerable.Empty<IStateItem>();
      }
   }
}
