// <copyright file="MeasurementProfile.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using AutoMapper;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.Web.Data;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Service.MapProfiles
{
   public class MeasurementProfile : Profile
   {
      public MeasurementProfile()
      {
         MapFromDTOToDomainObjects();
      }

      private void MapFromDTOToDomainObjects()
      {
         CreateMap<PropertyValueDTO, PropertyValue>();
         CreateMap<CharacteristicDTO, Characteristic>()
            .ConstructUsing(src => new Characteristic(src.Id, src.Name, Enum.GetName(typeof(CharacteristicType), src.CharacteristicType), null, null, null, null, null, null))
            .ForMember(dest => dest.Feature, org => org.Ignore());
         CreateMap<MeasurementResultLowDTO, RunDescriptor>();
      }
   }
}
