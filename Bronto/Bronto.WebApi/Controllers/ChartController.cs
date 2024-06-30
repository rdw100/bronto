using Bronto.Models;
using Bronto.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Bronto.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private IConfiguration _config { get; set; }

        private readonly ChartService _chartService;
        private readonly IMemoryCache _cache;

        public ChartController(IConfiguration iConfig)
        {
            _config = iConfig;
        }

        public ChartController(IConfiguration iConfig, IMemoryCache cache, ChartService chartService)
        {
            _config = iConfig;
            _cache = cache;
            _chartService = chartService;
        }

        // GET api/<ChartController>/AAPL
        [HttpGet]
        public async Task<ActionResult<List<MyOHLC>>> GetStockData(
            [FromQuery] string symbol,
            [FromQuery] string interval = "1d", // Default interval is 1 day
            [FromQuery] string range = "5d",   // Default range is 5 days
            [FromQuery] long? period1 = null,  // Default period1 is null (to be calculated)
            [FromQuery] long? period2 = null)  // Default period2 is null (to be calculated)
        {
            // Calculate default period1 and period2 if not provided
            if (!period1.HasValue)
            {
                // Get Unix timestamp for Monday of the current week
                var now = DateTimeOffset.UtcNow;
                var monday = now.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday);
                period1 = monday.ToUnixTimeSeconds();
            }

            if (!period2.HasValue)
            {
                var now = DateTimeOffset.UtcNow;
                // Get Unix timestamp for Friday of the current week
                var friday = now.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Friday);
                period2 = friday.ToUnixTimeSeconds();
            }

            try
            {
                var response = await _chartService.GetStockData(symbol, interval, range, period1, period2);
                return Ok(response);
            }
            catch (HttpRequestException)
            {
                // Handle exceptions (e.g., network issues)
                return StatusCode(500); // Internal Server Error
            }
        }
    }
}