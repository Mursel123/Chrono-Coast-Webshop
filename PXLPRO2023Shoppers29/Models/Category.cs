using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PXLPRO2023Shoppers29.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        public ICollection<Product> Products { get; set; }
    }

}
