using Mitutoyo.MiCAT.DataServiceClient;
using System.Diagnostics.CodeAnalysis;

namespace Mitutoyo.Micat.PdfExportDebugModule.Persistence
{
   [ExcludeFromCodeCoverage]
   public class DataServiceConfigurator : IDataServiceConfigurator
   {
      private readonly IDataServiceClient _dataServiceClient;

      public DataServiceConfigurator(IDataServiceClient dataServiceClient)
      {
         _dataServiceClient = dataServiceClient;
      }

      public void Configure()
      {
         _dataServiceClient.SetDataServiceUri("http://localhost/micatdataservice/");
      }
   }
}
