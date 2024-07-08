using System.Data;
using Microsoft.EntityFrameworkCore;
using ReportService.Data;

namespace ReportService.Services
{
    public class DataService : IDataService
    {
        private readonly DatabaseContext _context;

        public DataService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<DataTable> GetReportDataAsync(int transactionId)
        {
            var dataTable = new DataTable();
            var commandText = "SELECT * FROM SalesData WHERE TransactionId = @transactionId";

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = commandText;
                var parameter = command.CreateParameter();
                parameter.ParameterName = "@transactionId";
                parameter.Value = transactionId;
                command.Parameters.Add(parameter);

                using var reader = await command.ExecuteReaderAsync();
                dataTable.Load(reader);
            }
            return dataTable;
        }

        public async Task<List<SalesDataDto>> GetTransactionsAsync()
        {
            return await _context.SalesData
                .Select(s => new SalesDataDto
                {
                    TransactionID = s.TransactionID,
                    ProductName = s.ProductName,
                    Quantity = s.Quantity ?? 0,
                    Price = s.Price ?? 0,
                    TransactionDate = s.TransactionDate ?? DateTime.MinValue
                })
                .ToListAsync();
        }
    }
}