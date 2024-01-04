using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockApi.Data;
using StockApi.Models;
using StockApi.Services;
using StockApi.ViewModels;
using System;

namespace StockApi.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        private readonly DataContext _context;
        public StockController(IStockRepository stockRepo, DataContext context)
        {
            _stockRepo = stockRepo;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Stock> stock = _stockRepo.GetAll();
            List<StockOutputModel> models = stock.Select(s => StockOutputModel.FromStock(s)).ToList();
            return Ok(models);
        }

        [HttpGet("{serialNumber}")]
        public IActionResult GetDetails(string serialNumber)
        {
            Stock stock = _stockRepo.GetBySerialNumber(serialNumber);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(StockOutputModel.FromStock(stock));
        }

        [HttpPost]
        public IActionResult AddStock(Stock stock)
        {
            _stockRepo.Add(stock);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStock(Stock model)
        {
            _stockRepo.Update(model);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{serialNumber}")]
        public IActionResult DeleteStock(string serialNumber)
        {
            Stock stock = _stockRepo.GetBySerialNumber(serialNumber);
            if (stock == null)
            {
                return NotFound();
            }
            _stockRepo.Delete(stock);
            return Ok();
        }

    }
}
