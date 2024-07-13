using Bronto.Models.Api.Chart;
using Bronto.Shared;
using Bronto.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Bronto.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private IConfiguration _config { get; set; }
        private readonly IMemoryCache _cache;
        private readonly IChartService _chartService;

        public ChartController(IConfiguration iConfig, IMemoryCache cache, IChartService chartService)
        {
            _config = iConfig;
            _cache = cache;
            _chartService = chartService;
        }

        // GET: api/Chart/{symbol}
        [HttpGet]
        public async Task<ActionResult<ChartResult>> GetChartData(
            [FromQuery] string symbol,
            [FromQuery] string interval = "1d", // Default interval is 1 day
            [FromQuery] string range = "5d",   // Default range is 5 days
            [FromQuery] long? period1 = null,  // Default period1 is null (to be calculated)
            [FromQuery] long? period2 = null)
        {
            // Calculate default period1 and period2 if not provided
            if (!period1.HasValue || !period2.HasValue)
            {
                CalculateStartEnd(out period1, out period2);
            }
            
            // Retrieve stock by symbol
            try
            {
                var _response = await _chartService.GetChartData(symbol, interval, range, period1, period2);

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

        private static void CalculateStartEnd(out long? period1, out long? period2)
        {
            DateTime today = DateTime.UtcNow;
            var calculator = new UnixTimestampCalculator(today);

            period1 = calculator.MondayUnixTime;
            period2 = calculator.FridayUnixTime;
        }
    }
}