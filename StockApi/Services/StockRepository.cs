using StockApi.Data;
using StockApi.Models;
using System;

namespace StockApi.Services
{
    public class StockRepository : IStockRepository
    {
        private readonly DataContext _context;
        public StockRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(Stock stock)
        {
            _context.Add(stock);
            _context.SaveChanges();
        }

        public void Delete(Stock stock)
        {
            _context.Remove(stock);
            _context.SaveChanges();
        }

        public IEnumerable<Stock> GetAll()
        {
            return _context.Stock.ToList();
        }

        public Stock GetBySerialNumber(string serialNumber)
        {
            return _context.Stock.FirstOrDefault(s => s.SerialNumber == serialNumber);
        }

        public void Update(Stock stock)
        {
            _context.Update(stock);
            _context.SaveChanges();
        }
    }
}
