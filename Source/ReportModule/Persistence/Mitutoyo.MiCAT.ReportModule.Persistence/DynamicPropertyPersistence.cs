// <copyright file="DynamicPropertyPersistence.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service;
using Mitutoyo.MiCAT.Web.Data;

namespace Mitutoyo.MiCAT.ReportModule.Persistence
{
   public class DynamicPropertyPersistence : IDynamicPropertyPersistence
   {
      private readonly IPersistenceDataService _service;
      private readonly IMapper _mapper;

      public DynamicPropertyPersistence(IPersistenceServiceLocator persistenceServiceLocator, IMapper mapper)
      {
         _service = persistenceServiceLocator.GetService<IPersistenceDataService>(Location.DataService, Mechanism.DataService);
         _mapper = mapper;
      }

      public async Task<IEnumerable<DynamicPropertyDescriptor>> GetDynamicProperties()
      {
         var result_measurmentResult = await _service.GetPropertyDefinitions(Web.Data.EntityType.MeasurementResult);
         var dynamicProperties = GetListOrEmpty<PropertyDefinitionDTO>(result_measurmentResult);
         return _mapper.Map<IEnumerable<PropertyDefinitionDTO>, IEnumerable<DynamicPropertyDescriptor>>(dynamicProperties);
      }

      private IEnumerable<T> GetListOrEmpty<T>(Result result)
      {
         return result is SuccessResult<IEnumerable<T>> succesResult ? succesResult.Result : Enumerable.Empty<T>();
      }
   }
}
