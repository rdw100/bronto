namespace Bronto.WebApi.Controllers
{
    using Bronto.WebApi.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class QuoteController : ControllerBase
    {
        private IConfiguration config { get; set; }
        private readonly IMemoryCache cache;
        private readonly IQuoteService quoteService;

        /// <summary>
        /// Handles requests for stock quote data.
        /// </summary>
        /// <param name="iConfig">API configuration with AppSettings</param>
        /// <param name="iCache">An in-memory caching service</param>
        /// <param name="iQuoteService">A service for key finance data</param>
        public QuoteController(IConfiguration iConfig, IMemoryCache iCache, IQuoteService iQuoteService)
        {
            config = iConfig;
            cache = iCache;
            quoteService = iQuoteService;
        }

        /// <summary>
        /// Retrieves key finance data for specified stock symbol(s) from external API.
        /// </summary>
        /// <param name="symbol">A comma-separated stock symbols</param>
        /// <returns>Returns key finance data for specified stock symbol(s).</returns>
        [HttpGet]
        public async Task<IActionResult> GetQuote(string symbol)
        {
            var quote = await quoteService.GetQuote(symbol);
            return Ok(quote);
        }
    }
}