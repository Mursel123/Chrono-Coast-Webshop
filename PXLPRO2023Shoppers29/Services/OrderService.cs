using PXLPRO2023Shoppers29.Models;
using PXLPRO2023Shoppers29.Services.Interfaces;
using PXLPRO2023Shoppers29.Data;
using Microsoft.EntityFrameworkCore;

namespace PXLPRO2023Shoppers29.Services
{
    public class OrderService : IOrderService
    {
        private readonly ShopDbContext _dbContext;

        public OrderService(ShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Order GetOrderById(int orderId)
        {
            return _dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Payment)
                .Include(o => o.OrderLines)
                .FirstOrDefault(o => o.OrderId == orderId);
        }

        public void CreateOrder(Order order)
        {
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
        }

        public void DeleteOrder(int orderId)
        {
            var order = GetOrderById(orderId);
            _dbContext.Orders.Remove(order);
            _dbContext.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            _dbContext.Orders.Update(order);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _dbContext.Orders
                     .Include(o => o.Customer)
                     .Include(o => o.Payment)
                     .Include(o => o.OrderLines);
            
      }
    }
}
