using System;
using System.Diagnostics.CodeAnalysis;

namespace Mitutoyo.Micat.PdfExportDebugModule.Descriptors
{
   [ExcludeFromCodeCoverage]
   public class TemplateDescriptor
   {
      public Guid Id { get; }

      public TemplateDescriptor(Guid id, string description)
      {
         Id = id;
         Description = description;
      }

      public string Description { get; }

      public override string ToString()
      {
         return Description;
      }
   }
}
