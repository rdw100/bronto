namespace Bronto.WebApi.Controllers
{
    using Bronto.WebApi.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteService quoteService;

        public QuoteController(IQuoteService iQuoteService)
        {
            quoteService = iQuoteService;
        }

        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetQuote(string symbol)
        {
            var quote = await quoteService.GetQuote(symbol);
            return Ok(quote);
        }
    }
}