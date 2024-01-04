using PXLPRO2023Shoppers29.Models;

namespace PXLPRO2023Shoppers29.Services.Interfaces
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> GetCartAsync(int customerId);
        Task<IEnumerable<ShoppingCart>> GetShoppingCartItemsAsync(int customerId);
        Task AddToCartAsync(int customerId, int productId, int quantity);
        Task RemoveFromCartAsync(int customerId, int productId);
        Task UpdateQuantityAsync(int customerId, int quantity);
        Task<Product> GetProductByIdAsync(int productId);
        Task<int> GetItemCountAsync(int customerId);
        Task<decimal> GetCartTotalAsync(int customerId);
        Task ClearCartAsync(int customerId);



        Task CheckOutAsync(int customerId);


    }





}
