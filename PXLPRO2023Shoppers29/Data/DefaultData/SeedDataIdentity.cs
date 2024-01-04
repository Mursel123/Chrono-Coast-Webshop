using Microsoft.AspNetCore.Identity;
using PXLPRO2023Shoppers29.Models;
using System.Data;

namespace PXLPRO2023Shoppers29.Data.DefaultData
{
    public class SeedDataIdentity
    {

        static ShopDbContext? _context;
        static UserManager<ShopUser>? _userManager;
        static RoleManager<IdentityRole>? _roleManager;

        public static async Task EnsurePopulatedAsync(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                _context = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
                _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ShopUser>>();
                _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await CreateAllRoles();
                await AddProductAsync();

            }
        }

        //add new watches to the product 
        
        

        private static async Task CreateAllRoles()
        {
            if (_roleManager != null && !_roleManager.Roles.Any())
            {
                await CreateRole(Roles.Client);
                await CreateRole(Roles.Admin);
            }
        }
        private static async Task CreateRole(string rolename)
        {
            if (_roleManager != null && !await _roleManager.RoleExistsAsync(rolename))
            {
                var role = new IdentityRole() { Name = rolename };
                await _roleManager.CreateAsync(role);
            }
        }
       

        //add new watches to the product
        private static async Task AddProductAsync()
        {
            if (_context != null && !_context.ProductCategories.Any())
            {
                Category category1 = new Category() { CategoryName = "Sport" };
                Category category2 = new Category() { CategoryName = "Luxury" };
                Category category3 = new Category() { CategoryName = "Dress" };
                await _context.AddRangeAsync(category1, category2, category3);
                await _context.SaveChangesAsync();
            }
            if (_context != null && !_context.Products.Any())
            {
                await _context.Products.AddRangeAsync(
                     new Product
                     {
                         Name = "Rolex Submariner",
                         Price = 10000,
                         Description = "Iconic diving watch from Rolex",
                         Model = "116610LN",
                         Material = "Stainless Steel",
                         Bezel = "Ceramic",
                         SerialNumber = "1234567890",
                         MadeDate = "2022-01-01",
                         Dial = "Black",
                         Bracelet = "Oyster",
                         ImageUrl = "https://content.rolex.com/dam/2022-11/upright-cc/m136660-0004.png?impolicy=main-configurator&imwidth=680",
                         CategoryId = 1
                     },

                    new Product
                    {
                        Name = "Patek Philippe Nautilus",
                        Price = 50000,
                        Description = "Elegant sports watch from Patek Philippe.",
                        Model = "5711/1A-010",
                        Material = "Stainless Steel",
                        Bezel = "Fixed",
                        SerialNumber = "0987654321",
                        MadeDate = "2022-02-01",
                        Dial = "Blue",
                        Bracelet = "Stainless Steel",
                        ImageUrl = "/images/patek.png",
                        CategoryId = 2
                    },
                    new Product
                    {
                        Name = "Audemars Piguet Royal Oak",
                        Price = 40000,
                        Description = "Classic sports watch from Audemars Piguet.",
                        Model = "15400ST.OO.1220ST.02",
                        Material = "Stainless Steel",
                        Bezel = "Fixed",
                        SerialNumber = "1357908642",
                        MadeDate = "2022-03-01",
                        Dial = "Blue",
                        Bracelet = "Stainless Steel",
                        ImageUrl = "https://www.audemarspiguet.com/content/dam/ap/com/products/watches/MTR009891.00/importer/standup.png.transform.approductmain.png",
                        CategoryId = 2
                    },
                     new Product
                     {
                         Name = "Jaeger-LeCoultre Reverso",
                         Price = 20000,
                         Description = "Iconic Art Deco watch from Jaeger-LeCoultre.",
                         Model = "Q3738420",
                         Material = "Stainless Steel",
                         Bezel = "Fixed",
                         SerialNumber = "2468135790",
                         MadeDate = "2022-04-01",
                         Dial = "Silver",
                         Bracelet = "Leather",
                         ImageUrl = "https://img.jaeger-lecoultre.com/product-card-md-3/27e636a5ed4c82b0888e283dac08895e458e871f.jpg",
                         CategoryId = 3
                     },
                     new Product
                     {
                         Name = "Rolex Day-Date 40",
                         Price = 38500,
                         Description = "The Oyster Perpetual Day-Date 40 in 18 ct yellow gold with a silver dial, fluted bezel and a President bracelet.",
                         Model = "M228238-0042",
                         Material = "Yellow Gold",
                         Bezel = "Fixed",
                         SerialNumber = "ZE8K2Y6N",
                         MadeDate = "2021-05-26",
                         Dial = "Silver",
                         Bracelet = "Yellow Gold",
                         ImageUrl = "https://content.rolex.com/v7/dam/2023/upright-bba-with-shadow/m228238-0066.png?impolicy=v7-grid&imwidth=320",
                         CategoryId = 3



                     }
                     

                    );
               
                await _context.SaveChangesAsync();
            }
        }
    }
}








