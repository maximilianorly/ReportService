using System;
using ClosedXML.Excel;
using System.Data;

namespace ReportService.Services
{
    public class ExcelService : IExcelService
    {
        public byte[] GenerateExcelFromData(DataTable data)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Report");
                var currentRow = 1;

                // Adding column headers
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    worksheet.Cell(currentRow, i + 1).Value = data.Columns[i].ColumnName;
                    worksheet.Cell(currentRow, i + 1).Style.Font.Bold = true;
                }

                // Adding rows
                foreach (DataRow row in data.Rows)
                {
                    currentRow++;
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        worksheet.Cell(currentRow, i + 1).Value = row[i] != null ? row[i].ToString() : "";
                    }
                }

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }
}

