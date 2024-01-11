using Mitutoyo.Micat.PdfExportDebugModule.Descriptors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mitutoyo.Micat.PdfExportDebugModule.Persistence
{
   public interface IMainPersistence
   {
      Task<IEnumerable<RunDescriptor>> GetRuns();
      Task<IEnumerable<TemplateDescriptor>> GetTemplates();
   }
}