using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PXLPRO2023Shoppers29.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        [DisplayName("Customer")]
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        [Required]
        [DisplayName("Payment")]
        public int? PaymentId { get; set; }
        public Payment? Payment { get; set; }

        [Required]
        [DisplayName("Date")]
        [DataType(DataType.Date)]
        public DateTime? OrderDate { get; set; }

        [Required]
        [DisplayName("Total Price")]
        public decimal? TotalPrice { get; set; }


        [ForeignKey("OrderId")]
        public ICollection<OrderLine> OrderLines { get; set; }

        [Required]
        [DisplayName("Quantity")]
        public int Quantity { get; set; }


    }
}
