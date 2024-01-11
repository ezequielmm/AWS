// <copyright file="DataServiceStateDataLoader.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Persistence;

namespace Mitutoyo.MiCAT.ReportModule.Setup
{
   public class DataServiceStateDataLoader
   {
      private readonly IPartPersistence _partPersistence;
      private readonly IPlanPersistence _planPersistence;
      private readonly ICharacteristicDetailsProvider _detailsProvider;
      private readonly IDynamicPropertyPersistence _dynamicPropertyPersistence;
      private readonly IMeasurementPersistence _measurementPersistence;

      public DataServiceStateDataLoader(
         IPartPersistence partPersistence,
         IPlanPersistence planPersistence,
         IDynamicPropertyPersistence dynamicPropertyPersistence,
         IMeasurementPersistence measurementPersistence,
         ICharacteristicDetailsProvider detailsProvider)
      {
         _partPersistence = partPersistence;
         _planPersistence = planPersistence;
         _dynamicPropertyPersistence = dynamicPropertyPersistence;
         _measurementPersistence = measurementPersistence;
         _detailsProvider = detailsProvider;
      }

      public async Task<DataServiceSetupData> GetData()
      {
         var plans = await GetPlanDescriptorsWithPartId();
         var dynamicPropertiesTask = _dynamicPropertyPersistence.GetDynamicProperties();
         var characteristicTypes = _measurementPersistence.GetCharacteristicTypes();
         var details = _detailsProvider.GetAllCharacteristicDetails();

         return new DataServiceSetupData
         {
            Plans = plans,
            DynamicProperties = await dynamicPropertiesTask,
            CharacteristicsTypes = characteristicTypes,
            AllCharacteristicDetails = details
         };
      }

      private async Task<IEnumerable<PlanDescriptor>> GetPlanDescriptorsWithPartId()
      {
         var plans = await _planPersistence.GetPlans();
         return plans;
      }
   }
}
