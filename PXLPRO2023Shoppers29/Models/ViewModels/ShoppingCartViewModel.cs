using PXLPRO2023Shoppers29.Models;

namespace PXLPRO2023Shoppers29.Models.ViewModels
{
    internal class ShoppingCartViewModel
    {
        public ShoppingCart ShoppingCart { get; set; }
        public decimal ShoppingCartTotal { get; set; }
        public object ShoppingCartItems { get; internal set; }
        public int ShoppingCartItemsTotal { get; internal set; }
    }
}