using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockApi.Models
{
    [Index(nameof(SerialNumber), IsUnique = true)]
    public class Stock
    {
        [Key]
        public int StockId { get; set; }

        [Required]
        [DisplayName("Serial Number")]
        public string SerialNumber { get; set; }

        [Required]
        public int Amount { get; set; }
    }
}
