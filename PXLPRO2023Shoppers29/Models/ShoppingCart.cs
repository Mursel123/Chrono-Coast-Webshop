using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PXLPRO2023Shoppers29.Models
{
    public class ShoppingCart
    {
        [Key]
        public int ShoppingCartId { get; set; }

        [Required]
        [DisplayName("Customer")]
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

       
        [Required]
        [DisplayName("Quantity")]
        public int Quantity { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }

}

