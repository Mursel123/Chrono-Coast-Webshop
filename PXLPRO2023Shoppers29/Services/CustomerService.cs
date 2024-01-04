using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PXLPRO2023Shoppers29.Data;
using PXLPRO2023Shoppers29.Models;
using PXLPRO2023Shoppers29.Services.Interfaces;
using System.Security.Claims;

namespace PXLPRO2023Shoppers29.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ShopDbContext _dbContext;
        private readonly UserManager<ShopUser> _userManager;

        public CustomerService(ShopDbContext dbContext, UserManager<ShopUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _dbContext.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerById(int? id)
        {
            return await _dbContext.Customers.FindAsync(id);
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            _dbContext.Customers.Update(customer);
            await _dbContext.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> DeleteCustomer(int id)
        {
            var customer = await GetCustomerById(id);
            if (customer != null)
            {
                _dbContext.Customers.Remove(customer);
                await _dbContext.SaveChangesAsync();
            }
            return customer;
        }
        //GetCurrentCustomerId
        public async Task<string> GetCurrentCustomerId(HttpContext context)
        {
            var userId = _userManager.GetUserId(context.User);
            var customerId = await GetCustomerIdByUserIdAsync(userId);
            return customerId.ToString();
        }

        public CustomerService(UserManager<ShopUser> userManager, ShopDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<int?> GetCustomerIdByUserIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            var customer = await _dbContext.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == user.CustomerId);

            return customer?.CustomerId;
        }
    }

}

        
