using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PXLPRO2023Shoppers29.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [DisplayName("First Name")]
        public string? FirstName { get; set; }

        [DisplayName("Last Name")]
        public string? LastName { get; set; }


        [DisplayName("Phone Number")]
        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        [DisplayName("Zip Code")]
        public string? ZipCode { get; set; }

      public virtual List<ShoppingCart>? ShoppingCarts { get; set; }

    }

}
