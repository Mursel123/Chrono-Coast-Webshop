using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PXLPRO2023Shoppers29.Data;
using PXLPRO2023Shoppers29.Data.DefaultData;
using PXLPRO2023Shoppers29.Models;
using PXLPRO2023Shoppers29.Models.ViewModels;
using PXLPRO2023Shoppers29.Services.Interfaces;
using System.Diagnostics;

namespace PXLPRO2023Shoppers29.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        //[Authorize]
        public async Task<IActionResult> IndexAsync()
        {
            var cartId = HttpContext.Session.GetString("CartId");

            if (cartId == null)
            {
                cartId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("CartId", cartId);
            }

            // Pass the cartId to the view
            ViewData["CartId"] = cartId;

            var products = await _productService.GetAllProducts();


            return View(products);

            //return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("Home/BlazorProduct")]
        public IActionResult BlazorProduct()
        {
            return View();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("Home/BlazorAddProduct")]
        public IActionResult BlazorAddProduct()
        {
            return View();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("Home/BlazorCategory")]
        public IActionResult BlazorCategory()
        {
            return View();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("Home/BlazorAddCategory")]
        public IActionResult BlazorAddCategory()
        {
            return View();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("Home/BlazorEditProduct/{productId}")]
        public IActionResult BlazorEditProduct([FromRoute] int productId)
        {

            return View(productId);
        }



    }
}