using PXLPRO2023Shoppers29.Models;

namespace PXLPRO2023Shoppers29.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task<Product> AddProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task<Product> DeleteProduct(int id);
        Task<Product> GetProductByIdAsync(int id);
    }


}
