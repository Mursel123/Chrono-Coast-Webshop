using Microsoft.AspNetCore.Identity;

namespace PXLPRO2023Shoppers29.Models
{
    public class ShopUser : IdentityUser
    {
        public int? CustomerId { get; set; }
    }
}
