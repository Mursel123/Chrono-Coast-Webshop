using Microsoft.EntityFrameworkCore;
using StockApi.Models;


namespace StockApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> opts) : base(opts) { }

        public DbSet<Stock> Stock { get; set; }
    }
}
