// <copyright file="StreamHelperTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Mitutoyo.MiCAT.ReportModule.Persistence.Helpers;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Test.Helper
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class StreamHelperTest
   {
      [Test]
      public void GetByteArrayFromStream_From_String_Test()
      {
         // Arrange
         var ms = new MemoryStream(Encoding.ASCII.GetBytes("test"));

         // Act

         var bytes = StreamHelper.GetByteArrayFromStream(ms);

         // Assert
         Assert.AreEqual(Encoding.ASCII.GetBytes("test"), bytes);
      }

      [Test]
      public void GetByteArrayFromStream_From_File_Test()
      {
         // Arrange
         var fs = new FileStream("Helper\\test.txt", FileMode.Open);
         var encodedBytes = Encoding.UTF8.GetBytes("test");

         // Act
         var bytes = StreamHelper.GetByteArrayFromStream(fs);

         // Assert
         Assert.AreEqual(encodedBytes, bytes);
      }
   }
}
