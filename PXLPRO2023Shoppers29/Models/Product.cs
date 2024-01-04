using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PXLPRO2023Shoppers29.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
       
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Material { get; set; }
        [Required]
        public string Bezel { get; set; }
        [Required]
        [DisplayName("Serial Number")]
        public string SerialNumber { get; set; }
        [Required]
        [DisplayName("Made Date")]
        public string MadeDate { get; set; }
        [Required]
        public string? Dial { get; set; }
        [Required]
        public string? Bracelet { get; set; }
        [Required]
        public string? ImageUrl { get; set; }

        [Required]
        [DisplayName("Category")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
       
        [JsonIgnore]
        public ICollection<OrderLine> OrderLines { get; set; }





    }
}
