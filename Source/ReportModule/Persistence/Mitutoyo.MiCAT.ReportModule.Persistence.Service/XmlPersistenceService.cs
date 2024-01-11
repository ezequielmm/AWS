// <copyright file="XmlPersistenceService.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.IO;
using System.Xml.Serialization;
using Mitutoyo.MiCAT.ReportModule.Domain.DataResult;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Service
{
   public class XmlPersistenceService<T> : IFilePersistenceService<T>
   {
      public Result<T> Load(string fileName)
      {
         if (string.IsNullOrEmpty(fileName))
            return new Result<T>((T)Activator.CreateInstance(typeof(T)),
               new ResultInfo(ResultState.Error, "Invalid path or file name specified"));
         FileStream fs = new FileStream(fileName, FileMode.Open);
         return ConvertToResult(fs);
      }

      public void Save(T data, string fileName)
      {
         ValidateFileName(fileName);
         XmlSerializer serializer = new XmlSerializer(typeof(T));
         TextWriter writer = new StreamWriter(fileName);
         serializer.Serialize(writer, data);
         writer.Close();
      }

      private void ValidateFileName(string fileName)
      {
         if (string.IsNullOrEmpty(fileName))
            throw new ArgumentNullException("File name cannot be null");
      }
      private Result<T> ConvertToResult(FileStream fs)
      {
         try
         {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return new Result<T>((T)serializer.Deserialize(fs), new ResultInfo(ResultState.Success));
         }
         catch
         {
            return new Result<T>((T)Activator.CreateInstance(typeof(T)),
               new ResultInfo(ResultState.Error, "Error deserializing XML"));
         }
      }
   }
}
