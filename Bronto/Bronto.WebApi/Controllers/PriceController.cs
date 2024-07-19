using Bronto.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Bronto.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly IPriceService priceService;
        private IConfiguration config { get; set; }
        private readonly IMemoryCache cache;
        protected internal string Key { get; set; }
        protected internal string Host { get; set; }

        public PriceController(IConfiguration iConfig, IMemoryCache iCache, IPriceService iPriceService)
        {
            config = iConfig;
            cache = iCache;
            Key = config.GetSection("AppSettings")["Key"];
            Host = config.GetSection("AppSettings")["Host"];
            priceService = iPriceService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string symbol)
        {
            // Retrieve stock by symbol
            try
            {
                var _response = await priceService.GetPriceData(symbol);

                if (_response == null)
                {
                    // 404 Not Found - No Resource 
                    return NotFound();
                }

                // Return a single stock
                return Ok(_response);
            }
            catch (HttpRequestException)
            {
                // Handle exceptions (e.g., network issues)
                return StatusCode(500); // 500 Internal Server Error
            }            
        }
    }
}