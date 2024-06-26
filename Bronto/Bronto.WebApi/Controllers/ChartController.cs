using Microsoft.AspNetCore.Mvc;

namespace Bronto.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        private IConfiguration _config { get; set; }

        protected internal string _baseUrl { get; set; }

        public ChartController(IConfiguration iConfig)
        {
            _config = iConfig;
            _baseUrl = _config.GetSection("FreeApiV8")["Host"];
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri($"https://{_baseUrl}/");
            _httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
        }

        // GET api/<ChartController>/AAPL
        [HttpGet]
        public async Task<IActionResult> GetStockData(
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

            // Construct the API URL//{_baseUrl}
            var apiUrl = $"{symbol}?interval={interval}&range={range}&period1={period1}&period2={period2}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();

                    // Process the data or return it as-is
                    return Ok(data);
                }
                else
                {
                    // Handle the error condition
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (HttpRequestException)
            {
                // Handle exceptions (e.g., network issues)
                return StatusCode(500); // Internal Server Error
            }
        }
    }
}