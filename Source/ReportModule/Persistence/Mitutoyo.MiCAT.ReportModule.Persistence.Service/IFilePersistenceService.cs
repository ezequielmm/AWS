// <copyright file="IFilePersistenceService.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Service
{
   public interface IFilePersistenceService<T>: IPersistenceService
   {
      Result<T> Load(string fileName);
      void Save(T data, string fileName);
   }
}
