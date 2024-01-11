// <copyright file="PersistenceServiceLocator.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Domain.Report;
using Mitutoyo.MiCAT.ReportModule.Persistence.Components;
using Mitutoyo.MiCAT.ReportModule.Persistence.Service;
using Mitutoyo.MiCAT.Utilities.IoC;
using Unity;

namespace Mitutoyo.MiCAT.ReportModule.Persistence
{
   public class PersistenceServiceLocator : IPersistenceServiceLocator
   {
      private readonly IUnityContainer _iocContainer;
      public PersistenceServiceLocator(IUnityContainer iocContainer)
      {
         _iocContainer = iocContainer;
         var serviceRegistrar = new ServiceRegistrar(_iocContainer);

         serviceRegistrar.RegisterSingleton<IPersistenceDataService, PersistenceDataService>(GetRegistrationName(Location.DataService, Mechanism.DataService)); //DSApi - DS
         serviceRegistrar.Register<IFilePersistenceService<ReportTemplateSO>, XmlPersistenceService<ReportTemplateSO>>(GetRegistrationName(Location.File, Mechanism.Xml)); // XML - File
         serviceRegistrar.Register<IFilePersistenceService<ReportPdf>, PdfPersistenceService>(GetRegistrationName(Location.File, Mechanism.Pdf)); // PDF - File
      }

      public T GetService<T>(Location location, Mechanism mechanism)
      {
         return _iocContainer.Resolve<T>(GetRegistrationName(location, mechanism));
      }

      private string GetRegistrationName(Location location, Mechanism mechanism)
      {
         return location.ToString() + mechanism.ToString();
      }
   }
   public enum Location
   {
      File,
      DataService
   }
   public enum Mechanism
   {
      Xml,
      Pdf,
      DataService
   }
}
