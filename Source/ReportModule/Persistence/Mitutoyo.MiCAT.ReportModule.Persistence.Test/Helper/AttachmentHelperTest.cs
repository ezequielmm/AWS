// <copyright file="AttachmentHelperTest.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Mitutoyo.MiCAT.DataServiceClient;
using Mitutoyo.MiCAT.ReportModule.Persistence.Helper;
using Mitutoyo.MiCAT.Web.Data;
using NUnit.Framework;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Test.Helper
{
   [ExcludeFromCodeCoverage]
   [TestFixture]
   public class AttachmentHelperTest
   {
      [Test]
      public void GetCapture3DFromDto_ImageValid()
      {
         // Arrange
         var measurementAttachmentResult = new WithAttachments<MeasurementResultMediumDTO>();

         measurementAttachmentResult.Entity = new MeasurementResultMediumDTO() { Id = Guid.NewGuid(), PlanId = Guid.NewGuid(), TimeStamp = new DateTime() };

         var attachments = new List<AttachmentInfo>();

         var attachmentData = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAIAAACQd1PeAAAACXBIWXMAAAsTAAALEwEAmpwYAAAADElEQVQImWNIKyoCAAKMAUuJWnujAAAAAElFTkSuQmCC");

         attachments.Add(new AttachmentInfo()
         {
            Id = Guid.NewGuid(),
            Name = "attachment1",
            Data = new MemoryStream(attachmentData),
            Index = 0,
            Type = AttachmentType.Capture3D
         });

         measurementAttachmentResult.Attachments = attachments;

         // Act
         var captures3D = AttachmentHelper.GetCapture3DFromDto(measurementAttachmentResult);

         // Assert
         Assert.AreEqual(1, captures3D.Count);
         Assert.IsTrue(captures3D[0].IsValid);
         Assert.AreEqual("attachment1", captures3D[0].Name);
         Assert.AreEqual(0, captures3D[0].Index);
      }

      [Test]
      public void GetCapture3DFromDto_ImageInValid()
      {
         // Arrange
         var measurementAttachmentResult = new WithAttachments<MeasurementResultMediumDTO>();

         measurementAttachmentResult.Entity = new MeasurementResultMediumDTO() { Id = Guid.NewGuid(), PlanId = Guid.NewGuid(), TimeStamp = new DateTime() };

         var attachments = new List<AttachmentInfo>();

         //Invalid image attachment
         var attachmentData = Convert.FromBase64String("aW52YWxpZCBpbWFnZQ==");

         attachments.Add(new AttachmentInfo()
         {
            Id = Guid.NewGuid(),
            Name = "attachment1",
            Data = new MemoryStream(attachmentData),
            Index = 0,
            Type = AttachmentType.Capture3D
         });

         measurementAttachmentResult.Attachments = attachments;

         // Act
         var captures3D = AttachmentHelper.GetCapture3DFromDto(measurementAttachmentResult);

         // Assert
         Assert.AreEqual(1, captures3D.Count);
         Assert.IsFalse(captures3D[0].IsValid);
         Assert.AreEqual("attachment1", captures3D[0].Name);
         Assert.AreEqual(0, captures3D[0].Index);
      }
   }
}
