using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PXLPRO2023Shoppers29.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        [Required]
        [DisplayName("Payment Method")]
        public string PaymentMethod { get; set; }
        [Required]
        [DisplayName("Cardholder Name")]
        public string CardholderName { get; set; }
        [Required]
        [DisplayName("Card Number")]
        public string CardNumber { get; set; }
        [Required]
        [DisplayName("Expiration Date")]
        public string ExpirationDate { get; set; }
        [Required]
        [DisplayName("Security Code")]
        public string SecurityCode { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }

}
