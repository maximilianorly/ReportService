using Microsoft.AspNetCore.Mvc;
using ReportService.Services;

namespace ReportService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly IExcelService _excelService;
        private readonly IPDFService _pdfService;

        public ReportsController(IDataService dataService, IExcelService excelService, IPDFService pdfService)
        {
            _dataService = dataService;
            _excelService = excelService;
            _pdfService = pdfService;
        }

        [HttpGet("excel/{transactionId}")]
        public async Task<IActionResult> GenerateExcelReport(int transactionId)
        {
            try
            {
                var data = await _dataService.GetReportDataAsync(transactionId);

                if (data == null || data.Rows.Count == 0)
                {
                    return NotFound("No data found for the specified transaction.");
                }

                var excelFileContents = _excelService.GenerateExcelFromData(data);
                if (excelFileContents == null || excelFileContents.Length == 0)
                {
                    return StatusCode(500, "Failed to generate the Excel document.");
                }

                // Create and return file from contents
                return File(excelFileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("pdf/{transactionId}")]
        public async Task<IActionResult> GeneratePdfReport(int transactionId)
        {
            try
            {
                var data = await _dataService.GetReportDataAsync(transactionId);
                if (data == null || data.Rows.Count == 0)
                {
                    return NotFound("No data found for the specified transaction.");
                }

                var pdfFileContents = _pdfService.GeneratePdfFromData(data);
                if (pdfFileContents == null || pdfFileContents.Length == 0)
                {
                    return StatusCode(500, "Failed to generate the PDF file.");
                }

                // Create and return file from contents
                return File(pdfFileContents, "application/pdf", $"Report_{transactionId}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("transactions")]
        public async Task<ActionResult<List<SalesDataDto>>> GetTransactions()
        {
            try
            {
                var transactions = await _dataService.GetTransactionsAsync();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
