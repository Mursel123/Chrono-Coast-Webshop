using StockApi.Models;
using System;

namespace StockApi.ViewModels
{
    public class StockOutputModel : Stock
    {
        public static StockOutputModel FromStock(Stock stock)
        {
            return new StockOutputModel
            {
                StockId = stock.StockId,
                SerialNumber = stock.SerialNumber,
                Amount = stock.Amount
            };
        }

    }
}
