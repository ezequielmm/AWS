using Mitutoyo.Micat.PdfExportDebugModule.Descriptors;
using Mitutoyo.MiCAT.DataServiceClient;
using Mitutoyo.MiCAT.Web.Data;
using Mitutoyo.MiCAT.Web.Data.Common;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Mitutoyo.Micat.PdfExportDebugModule.Persistence
{
   [ExcludeFromCodeCoverage]
   public class MainPersistence : IMainPersistence
   {
      private readonly IDataServiceClient _dataServiceClient;

      public MainPersistence(IDataServiceClient dataServiceClient)
      {
         _dataServiceClient = dataServiceClient;
      }

      public async Task<IEnumerable<TemplateDescriptor>> GetTemplates()
      {
         var templatesDto = await _dataServiceClient.GetAllReportTemplates<ReportTemplateLowDTO>();
         return templatesDto.Success.Select(t => new TemplateDescriptor(t.Id, t.Name));
      }

      public async Task<IEnumerable<RunDescriptor>> GetRuns()
      {
         var plansDto = await _dataServiceClient.GetAllPlans<PlanLowDTO>(VersionFilter.Latest(), null);
         var runsDto = await _dataServiceClient.GetAllMeasurementResults<MeasurementResultLowDTO>();

         var runs = plansDto
            .Success
            .SelectMany(p => runsDto
                           .Success
                           .Where(r => r.PlanId == p.Id)
                           .Select(r => new RunDescriptor(r.Id, $"{p.Name} - {r.TimeStamp}"))
                        );
         return runs;
      }
   }
}
