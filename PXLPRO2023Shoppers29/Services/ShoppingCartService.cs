using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PXLPRO2023Shoppers29.Data;
using PXLPRO2023Shoppers29.Models;
using PXLPRO2023Shoppers29.Services.Interfaces;

namespace PXLPRO2023Shoppers29.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ShopDbContext _dbContext;
        private readonly IProductService _productService;
        private readonly UserManager<ShopUser> _userManager;
        private readonly IEmailService _emailService;

        public ShoppingCartService(ShopDbContext dbContext, IProductService productService, UserManager<ShopUser> userManager, IEmailService emailService)
        {
            _dbContext = dbContext;
            _productService = productService;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task AddToCartAsync(int customerId, int productId, int quantity)
        {
            var shoppingCart = await _dbContext.ShoppingCarts
                .Include(c => c.OrderLines)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart
                {
                    CustomerId = customerId,
                    Quantity = 1, // set to 0
                    OrderLines = new List<OrderLine>
            {
                new OrderLine { ProductId = productId, Quantity = quantity }
            }
                };

                _dbContext.ShoppingCarts.Add(shoppingCart);
            }
            else
            {
                var orderLine = shoppingCart.OrderLines.FirstOrDefault(ol => ol.ProductId == productId);

                if (orderLine == null)
                {
                    shoppingCart.OrderLines.Add(new OrderLine { ProductId = productId, Quantity = quantity });
                }
                else
                {
                    orderLine.Quantity += quantity;
                }

                shoppingCart.Quantity += quantity; // add the quantity of the order line
            }

            await _dbContext.SaveChangesAsync();
        }




        public async Task RemoveFromCartAsync(int customerId, int productId)
        {
            var shoppingCart = await _dbContext.ShoppingCarts
                .Include(c => c.OrderLines)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (shoppingCart == null)
            {
                return;
            }

            var orderLineToRemove = shoppingCart.OrderLines.FirstOrDefault(ol => ol.ProductId == productId);
            if (orderLineToRemove != null)
            {
                shoppingCart.OrderLines.Remove(orderLineToRemove);
                shoppingCart.Quantity -= 1;
                await _dbContext.SaveChangesAsync();
            }
            if (shoppingCart.Quantity == 0)
            {
                _dbContext.ShoppingCarts.Remove(shoppingCart);
                await _dbContext.SaveChangesAsync();
            }
            
            await _dbContext.SaveChangesAsync();

        }










        public async Task UpdateQuantityAsync(int customerId, int quantity)
        {
            var shoppingCart = await _dbContext.ShoppingCarts
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (shoppingCart != null)
            {
                shoppingCart.Quantity = quantity;
                await _dbContext.SaveChangesAsync();
            }
        }


        public async Task<ShoppingCart> GetCartByCustomerIdAsync(int customerId)
        {
            var cart = await _dbContext.ShoppingCarts
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    CustomerId = customerId
                };
                _dbContext.ShoppingCarts.Add(cart);
                await _dbContext.SaveChangesAsync();
            }

            return cart;
        }


        public async Task<ShoppingCart> GetCartAsync(int customerId)
        {
            var cart = await _dbContext.ShoppingCarts
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (cart == null)
            {
                cart = new ShoppingCart { CustomerId = customerId };
                _dbContext.ShoppingCarts.Add(cart);
                await _dbContext.SaveChangesAsync();
            }

            return cart;
        }


        public async Task<int> GetItemCountAsync(int customerId)
        {
            var cart = await _dbContext.ShoppingCarts
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            return cart?.Quantity ?? 0;
        }



        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _productService.GetProductByIdAsync(productId);
        }

        public async Task<IEnumerable<ShoppingCart>> GetShoppingCartItemsAsync(int customerId)
        {
            if (customerId == null || customerId == -1)
            {
                return null;
            }

            var cart = await _dbContext.ShoppingCarts
                      .Include(c => c.OrderLines)
                      .ThenInclude(ol => ol.Product)
                      .Where(c => c.CustomerId == customerId)
                      .ToListAsync();

            return cart;

        }


        public async Task CheckOutAsync(int customerId)
        {
            // Get the customer's cart
            var cart = _dbContext.ShoppingCarts
                .Include(c => c.OrderLines)
                .FirstOrDefault(c => c.CustomerId == customerId);

            // Create a new order from the cart
            var order = new Order
            {
                CustomerId = customerId,
                OrderLines = cart.OrderLines,
                TotalPrice = cart.OrderLines.Sum(ol => ol.Product.Price * ol.Quantity)
            };

            // Add the order to the database
            _dbContext.Orders.Add(order);

            // Remove the cart from the database
            _dbContext.ShoppingCarts.Remove(cart);

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

            // Send an email to the user with the invoice
            var user = await _userManager.FindByIdAsync(customerId.ToString());
            if (user != null)
            {
                await _emailService.SendEmailAsync(user.Email, "Order confirmation", $"Thank you for your order. Your order number is {order.OrderId}.");

            }
        }

        public async Task<decimal> GetCartTotalAsync(int customerId)
        {
            var cart = await _dbContext.ShoppingCarts
                .Include(c => c.OrderLines)
                .ThenInclude(ol => ol.Product)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            var total = cart.OrderLines.Sum(ol => ol.Product.Price * ol.Quantity);

            return (decimal)total;
        }

        public async Task ClearCartAsync(int customerId)
        {
            // Find the cart
            var cart = await _dbContext.ShoppingCarts
                .Include(c => c.OrderLines)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            // If the cart exists, remove it
            if (cart != null)
            {
                _dbContext.ShoppingCarts.Remove(cart);
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
