using System.Data;

namespace ReportService.Services
{
    public interface IPDFService
    {
        byte[] GeneratePdfFromData(DataTable data);
    }
}
