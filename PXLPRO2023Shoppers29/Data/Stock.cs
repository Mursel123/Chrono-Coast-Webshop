using System.ComponentModel;

namespace PXLPRO2023Shoppers29.Data
{
    public class Stock
    {
        public int StockId { get; set; }

        [DisplayName("Serial Number")]
        public string SerialNumber { get; set; }

        public int Amount { get; set; }
    }
}
