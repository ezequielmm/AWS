using Prism.Mvvm;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Mitutoyo.Micat.PdfExportDebugModule.Descriptors
{
   [ExcludeFromCodeCoverage]
   public class RunDescriptor: BindableBase
   {
      public RunDescriptor(Guid id, string description)
      {
         Id = id;
         Description = description;
      }

      public Guid Id { get; }
      public string Description { get; }
      

      private bool isSelected;

      public bool IsSelected
      {
         get { return isSelected; }
         set
         {
            isSelected = value;
            this.RaisePropertyChanged("IsSelected");
         }
      }

      public override string ToString()
      {
         return Description;
      }
   }
}
