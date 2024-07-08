using DinkToPdf;
using DinkToPdf.Contracts;
using System.Data;
using System.Reflection;
using System.Text;

namespace ReportService.Services
{

    public class PDFService : IPDFService
    {
        private readonly IConverter _converter;

        public PDFService(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] GeneratePdfFromData(DataTable data)
        {
            string htmlContent = LoadAndFillDemoTemplate(data, "DemoReport.html");

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = {
                    new ObjectSettings()
                    {
                        PagesCount = true,
                        HtmlContent = htmlContent,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            return _converter.Convert(doc);
        }

        private static string LoadAndFillDemoTemplate(DataTable data, string templateName)
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            if (string.IsNullOrEmpty(assemblyLocation))
            {
                throw new InvalidOperationException("Cannot find the location of the executing assembly.");
            }

            var directoryPath = Path.GetDirectoryName(assemblyLocation);
            if (directoryPath == null)
            {
                throw new InvalidOperationException("Cannot determine the directory of the executing assembly.");
            }

            var path = Path.Combine(directoryPath, "ReportsTemplates", templateName);
            string htmlTemplate = File.ReadAllText(path);

            // Outline html per row
            StringBuilder transactionDetail = new();
            foreach (DataRow row in data.Rows)
            {
                transactionDetail.Append("<div class='transaction'>");
                transactionDetail.Append($"<h3>Transaction ID: {row["TransactionID"]}</h3>");
                transactionDetail.Append("<div class='transaction-details'>");
                transactionDetail.Append($"<span class='highlight'>Product Name:</span> <span>{row["ProductName"]}</span>");
                transactionDetail.Append($"<span class='highlight'>Quantity:</span> <span>{row["Quantity"]}</span>");
                transactionDetail.Append($"<span class='highlight'>Price per unit:</span> <span>${row["Price"]}</span>");
                transactionDetail.Append($"<span class='highlight'>Date of transaction:</span> <span>{row["TransactionDate"]}</span>");
                transactionDetail.Append("</div>");
                transactionDetail.Append("</div>");
            }

            htmlTemplate = htmlTemplate.Replace("{{Date}}", DateTime.UtcNow.ToString("yyyy-MM-dd"));
            htmlTemplate = htmlTemplate.Replace("{{transactionDetail}}", transactionDetail.ToString());

            return htmlTemplate;
        }
    }
}