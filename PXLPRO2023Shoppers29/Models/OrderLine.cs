using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace PXLPRO2023Shoppers29.Models
{

    public class OrderLine
    {
        [Key]
        public int OrderLineId { get; set; }

        [Required]
        [DisplayName("ShoppingCart")]
        public int ShoppingCartId { get; set; }

        [Required]
        [DisplayName("Product")]
        public int? ProductId { get; set; }
        // circular reference to Product
        [JsonIgnore]
        public Product? Product { get; set; }

        [Required]
        [DisplayName("Quantity")]
        public int Quantity { get; set; }

        [Required]
        [DisplayName("Price")]
        public decimal Price { get; set; }

        
        
        
    }


}
