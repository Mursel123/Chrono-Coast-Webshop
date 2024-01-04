using PXLPRO2023Shoppers29.Data;

namespace PXLPRO2023Shoppers29.Services.Interfaces
{
    public interface IStockRepository
    {
        IEnumerable<Stock> GetAll();
        Stock GetBySerialNumber(string serialNumber);
        void Add(Stock stock);
        void Update(Stock stock);
        void Delete(string serialNumber);
    }
}
