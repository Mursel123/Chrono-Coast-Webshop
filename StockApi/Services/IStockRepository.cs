using StockApi.Models;

namespace StockApi.Services
{
    public interface IStockRepository
    {
        IEnumerable<Stock> GetAll();
        Stock GetBySerialNumber(string serialNumber);
        void Add(Stock stock);
        void Update(Stock stock);
        void Delete(Stock stock);


    }
}
