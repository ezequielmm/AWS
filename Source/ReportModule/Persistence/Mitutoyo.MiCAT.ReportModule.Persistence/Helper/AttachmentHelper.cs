// <copyright file="AttachmentHelper.cs" company="Mitutoyo Europe GmbH">
// Copyright (c) Mitutoyo Europe GmbH, All rights reserved
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System.Collections.Immutable;
using System.Linq;
using Mitutoyo.MiCAT.DataServiceClient;
using Mitutoyo.MiCAT.ReportModule.Domain.Data;
using Mitutoyo.MiCAT.ReportModule.Persistence.Helpers;
using Mitutoyo.MiCAT.Web.Data;

namespace Mitutoyo.MiCAT.ReportModule.Persistence.Helper
{
   public static class AttachmentHelper
   {
      public static ImmutableList<Capture3D> GetCapture3DFromDto(WithAttachments<MeasurementResultMediumDTO> measurementAttachmentResult)
      {
         return (measurementAttachmentResult.Attachments.Where(a => a.Type == AttachmentType.Capture3D).OrderBy(a => a.Index).Select(attachment => GetCapture3DFromAttachment(attachment))).ToImmutableList();
      }

      private static Capture3D GetCapture3DFromAttachment(AttachmentInfo attachmentInfo)
      {
         try
         {
            var image = ImageHelper.GetImageFromByteArray(StreamHelper.GetByteArrayFromStream(attachmentInfo.Data));
            return new Capture3D(attachmentInfo.Name, image, attachmentInfo.Index);
         }
         catch
         {
            return new Capture3D(attachmentInfo.Name, null, attachmentInfo.Index);
         }
      }
   }
}
