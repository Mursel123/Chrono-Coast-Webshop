using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PXLPRO2023Shoppers29.Data;
using PXLPRO2023Shoppers29.Models;
using PXLPRO2023Shoppers29.Models.ViewModels;
using PXLPRO2023Shoppers29.Services;
using PXLPRO2023Shoppers29.Services.Interfaces;
using System.Text.Json;

namespace PXLPRO2023Shoppers29.Controllers
{
    public class OrderController : Controller
    {    
        private readonly ShopDbContext _context;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IProductService _productService;
        private readonly IStockRepository _stockRepository;

        public OrderController(ShopDbContext context, IShoppingCartService shoppingCartService, IProductService productService, IStockRepository stockRepository)
        {
            _context = context;
            _shoppingCartService = shoppingCartService;
            _productService = productService;
            _stockRepository = stockRepository;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromForm] IFormCollection form)
        {
            int customerId = int.Parse(form["customerId"]);
            decimal orderTotal = decimal.Parse(form["totalPrice"]);
            string paymentMethod = form["payment-method"];

            var order = new Order
            {
                CustomerId = customerId,
                OrderDate = DateTime.Now,
                TotalPrice = orderTotal,
                OrderLines = new List<OrderLine>(),
                PaymentId = -1
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            
            var payment = new Payment
            {
                PaymentMethod = paymentMethod,
                CardholderName = "a",
                CardNumber = "a",
                ExpirationDate = "a",
                SecurityCode = "a",
                OrderId = order.OrderId

            };
            _context.Payments.Add(payment);
            
            _context.SaveChanges();


            
            order.Payment = payment;
            order.PaymentId = payment.PaymentId; 


            
            var orderLines = new List<OrderLine>();

            if (!string.IsNullOrEmpty(form["cartItems"]))
            {
                var cartItemsJson = form["cartItems"];
                var cartItems = JsonSerializer.Deserialize<List<OrderLine>>(cartItemsJson);

                foreach (var cartItem in cartItems)
                {
                    var orderLine = new OrderLine
                    {
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Price,
                        ShoppingCartId = cartItem.ShoppingCartId,
                        Product = cartItem.Product
                    };
                    var product = _productService.GetProductByIdAsync((int)cartItem.ProductId);
                    var stock = _stockRepository.GetBySerialNumber(product.Result.SerialNumber);
                    stock.Amount -= cartItem.Quantity;
                    _stockRepository.Update(stock);
                
                    
                    

                    orderLines.Add(orderLine);
                }
            }

            order.OrderLines = orderLines;
            order.Quantity = orderLines.Count();
            
            var orderViewModel = new OrderViewModel
            {
                OrderLines = orderLines,  
                CustomerId = customerId,
                TotalPrice = orderTotal,
                OrderDate = DateTime.Now,
                Payment = payment,

            };


            order.Customer = _context.Customers.Find(customerId);
            _context.SaveChanges();

            var AllOrders = _context.Orders.ToList();
            foreach (var Order in AllOrders)
            {
                if (Order.PaymentId == -1)
                {
                 
                    _context.Orders.Remove(Order);
                    _context.SaveChanges();
                }
               
            }
          

            
            await _shoppingCartService.ClearCartAsync(customerId);

            
            return View("PlaceOrder", new List<OrderViewModel> { orderViewModel });



        }
    }
}





