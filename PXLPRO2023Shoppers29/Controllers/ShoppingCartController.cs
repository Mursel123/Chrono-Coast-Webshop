using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Microsoft.EntityFrameworkCore;
using PXLPRO2023Shoppers29.Models;
using PXLPRO2023Shoppers29.Models.ViewModels;
using PXLPRO2023Shoppers29.Services.Interfaces;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PXLPRO2023Shoppers29.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IProductService _productService;
        private readonly UserManager<ShopUser> _userManager;
        private readonly ICustomerService _customerService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IProductService productService, UserManager<ShopUser> userManager, ICustomerService customerService)
        {
            _shoppingCartService = shoppingCartService;
            _productService = productService;
            _userManager = userManager;
            _customerService = customerService;
        }


        [Route("Checkout")]
        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);

            var customer = await _customerService.GetCustomerIdByUserIdAsync(user.Id);

            //get the shopping cart items
            var shoppingCart = await _shoppingCartService.GetCartAsync(customer.Value);

            return View(shoppingCart);


        }

        


        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetShoppingCartItems(int customerId)
        {
            var shoppingCartItems = await _shoppingCartService.GetShoppingCartItemsAsync(customerId);

            return Ok(shoppingCartItems);
        }

        [HttpPost("{productId}/{quantity}")]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            var customer = await _customerService.GetCustomerIdByUserIdAsync(user.Id);

            await _shoppingCartService.AddToCartAsync(customer.Value, productId, quantity);

            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart([FromForm] int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            var customer = await _customerService.GetCustomerIdByUserIdAsync(user.Id);

            await _shoppingCartService.RemoveFromCartAsync(customer.Value, productId);

            return RedirectToAction("Index", "Home");
        }


        [HttpPut("{quantity}")]
        public async Task<IActionResult> UpdateQuantity(int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            var customer = await _customerService.GetCustomerIdByUserIdAsync(user.Id);

            await _shoppingCartService.UpdateQuantityAsync(customer.Value, quantity);

            return Ok();
        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var customer = await _customerService.GetCustomerIdByUserIdAsync(user.Id);

            var shoppingCartItems = await _shoppingCartService.GetShoppingCartItemsAsync(customer.Value);

            return View(shoppingCartItems);



        }


       




    }

}
