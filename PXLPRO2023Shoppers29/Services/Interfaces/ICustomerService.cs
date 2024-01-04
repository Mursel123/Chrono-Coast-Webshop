using PXLPRO2023Shoppers29.Models;

namespace PXLPRO2023Shoppers29.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerById(int ? id);
        Task<Customer> AddCustomer(Customer customer);
        Task<Customer> UpdateCustomer(Customer customer);
        Task<Customer> DeleteCustomer(int id);
        Task<string> GetCurrentCustomerId(HttpContext context);
        Task<int?> GetCustomerIdByUserIdAsync(string userId);
        
    }

}
