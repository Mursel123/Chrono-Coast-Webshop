using PXLPRO2023Shoppers29.Data;
using PXLPRO2023Shoppers29.Data.DefaultData;
using PXLPRO2023Shoppers29.Services.Interfaces;
using System;

namespace PXLPRO2023Shoppers29.Services
{
    public class StockRepository : IStockRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public StockRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public void Add(Stock stock)
        {
            HttpClient client = _httpClientFactory.CreateClient(ApiConstants.StockApiHttpClientName);
            HttpResponseMessage response = client.PostAsJsonAsync("stock", stock).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not add stock");
            }
        }

        public void Delete(string serialNumber)
        {
            HttpClient client = _httpClientFactory.CreateClient(ApiConstants.StockApiHttpClientName);
            HttpResponseMessage response = client.DeleteAsync($"stock/{serialNumber}").Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not delete stock");
            }
        }

        public IEnumerable<Stock> GetAll()
        {
            HttpClient client = _httpClientFactory.CreateClient(ApiConstants.StockApiHttpClientName);
            HttpResponseMessage response = client.GetAsync("stock").Result;
            if (response.IsSuccessStatusCode)
            {
                IList<Stock> stocks = response.Content.ReadAsAsync<IList<Stock>>().Result;
                return stocks;
            }
            else
            {
                return Enumerable.Empty<Stock>();
            }
        }

        public Stock GetBySerialNumber(string serialNumber)
        {
            HttpClient client = _httpClientFactory.CreateClient(ApiConstants.StockApiHttpClientName);
            HttpResponseMessage response = client.GetAsync($"stock/{serialNumber}").Result;
            if (response.IsSuccessStatusCode)
            {
                Stock stock = response.Content.ReadAsAsync<Stock>().Result;
                return stock;
            }

            return null;
        }

        public void Update(Stock stock)
        {
            HttpClient client = _httpClientFactory.CreateClient(ApiConstants.StockApiHttpClientName);
            HttpResponseMessage response = client.PutAsJsonAsync("stock", stock).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not update stock");
            }

        }
    }
}
