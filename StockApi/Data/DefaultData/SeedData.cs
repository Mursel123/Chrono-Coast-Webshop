using Microsoft.EntityFrameworkCore;
using StockApi.Models;

namespace StockApi.Data.DefaultData
{
    public static class SeedData
    {
        public static void SeedDatabase(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                DataContext context = scope.ServiceProvider.GetRequiredService<DataContext>();
                SeedStockData(context);
            }
        }

        private static void SeedStockData(DataContext context)
        {
            context.Database.Migrate();
            if (!context.Stock.Any())
            {
                Stock s1 = new Stock { SerialNumber = "1234567890", Amount = 8 };
                Stock s2 = new Stock { SerialNumber = "0987654321", Amount = 5 };
                Stock s3 = new Stock { SerialNumber = "1357908642", Amount = 7 };
                Stock s4 = new Stock { SerialNumber = "2468135790", Amount = 6 };
                Stock s5 = new Stock { SerialNumber = "ZE8K2Y6N", Amount = 3 };
                context.Stock.AddRange(s1,s2, s3, s4, s5);
                context.SaveChanges();
            }

        }
    }
}
