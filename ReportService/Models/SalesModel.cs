using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportService.Models
{
    [Table("SalesData", Schema = "dbo")]
    public class SalesData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }

        [StringLength(100)]
        public string ProductName { get; set; } = "";

        public int? Quantity { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Price { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? TransactionDate { get; set; }
    }
}