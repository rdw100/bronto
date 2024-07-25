namespace Bronto.WebApi.Controllers
{
    using Bronto.WebApi.Services.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class QuoteController : ControllerBase
    {
        private readonly QuoteService _yahooFinanceService;

        public QuoteController(QuoteService yahooFinanceService)
        {
            _yahooFinanceService = yahooFinanceService;
        }

        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetQuote(string symbol)
        {
            var quote = await _yahooFinanceService.GetQuoteAsync(symbol);
            return Ok(quote);
        }
    }
}