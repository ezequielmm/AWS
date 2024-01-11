// <copyright file="AutoMapperConfig.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Persistence.MapProfiles;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service.MapProfiles;

namespace Mitutoyo.MiCAT.ReportModule.Setup.Configurations
{
   public static class AutoMapperConfig
   {
      public static AutoMapper.MapperConfiguration InitializeAutoMapper()
      {
         return new AutoMapper.MapperConfiguration(cfg =>
         {
            cfg.CreateMissingTypeMaps = true;
            cfg.AddProfile(new PersistenceProfile());
            cfg.AddProfile(new MeasurementProfile());
         });
      }
   }
}
