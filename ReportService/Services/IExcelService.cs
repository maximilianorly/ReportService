using System.Data;

namespace ReportService.Services
{
    public interface IExcelService
    {
        byte[] GenerateExcelFromData(DataTable data);
    }
}
