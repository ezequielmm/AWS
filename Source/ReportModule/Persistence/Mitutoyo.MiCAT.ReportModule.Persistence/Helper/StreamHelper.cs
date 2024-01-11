// <copyright file="StreamHelper.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.IO;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Helpers
{
   public static class StreamHelper
   {
      public static byte[] GetByteArrayFromStream(Stream inputStream)
      {
         if (inputStream is MemoryStream msResult)
            return msResult.ToArray();

         using var ms = new MemoryStream();
         inputStream.CopyTo(ms);
         return ms.ToArray();
      }
   }
}
