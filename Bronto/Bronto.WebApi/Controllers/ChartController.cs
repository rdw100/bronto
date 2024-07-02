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
        private readonly IMemoryCache _cache;
        private readonly ChartService _chartService;
        private List<MyOHLC>? _response;

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
            if (!period1.HasValue || !period2.HasValue)
            {
                DateTime monday, friday;
                CalculateStartEnd(out monday, out friday);

                // Convert to Unix timestamps
                period1 = (long)(monday - new DateTime(1970, 1, 1)).TotalSeconds;
                period2 = (long)(friday - new DateTime(1970, 1, 1)).TotalSeconds;
            }

            try
            {
                _response = await _chartService.GetStockData(symbol, interval, range, period1, period2);
                if (_response == null) {
                    // 404 Not Found - No Resource 
                    return NotFound();
                }
                return Ok(_response);
            }
            catch (HttpRequestException)
            {
                // Handle exceptions (e.g., network issues)
                return StatusCode(500); // 500 Internal Server Error
            }
        }

        private static void CalculateStartEnd(out DateTime monday, out DateTime friday)
        {
            DateTime today = DateTime.Today;

            // Check if today is a weekend (Saturday or Sunday)
            if (today.DayOfWeek == DayOfWeek.Saturday)
            {
                // Calculate previous Monday (5 days ago)
                monday = today.AddDays(-5);

                // Calculate previous Friday (1 days ago)
                friday = today.AddDays(-1);
            }
            else if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                // Calculate previous Monday (6 days ago)
                monday = today.AddDays(-6);

                // Calculate previous Friday (2 days ago)
                friday = today.AddDays(-2);
            }
            else
            {
                // Calculate current Monday
                monday = today.AddDays(-(int)today.DayOfWeek + 1);

                // Calculate current Friday
                friday = today.AddDays(5 - (int)today.DayOfWeek);
            }
        }
    }
}