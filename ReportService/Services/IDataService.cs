using System.Data;

namespace ReportService.Services
{
    public interface IDataService
    {
        Task<DataTable> GetReportDataAsync(int transactionId);
        Task<List<SalesDataDto>> GetTransactionsAsync();
    }
}
