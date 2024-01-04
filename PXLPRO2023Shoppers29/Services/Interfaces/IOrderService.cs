using PXLPRO2023Shoppers29.Models;
using PXLPRO2023Shoppers29.Models.ViewModels;

namespace PXLPRO2023Shoppers29.Services.Interfaces
{
    public interface IOrderService
    {
        Order GetOrderById(int orderId);
        void CreateOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(int orderId);
     
        IEnumerable<Order> GetAllOrders();
    }
}