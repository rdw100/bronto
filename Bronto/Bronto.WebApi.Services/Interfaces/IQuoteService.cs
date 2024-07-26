using Bronto.Models.Api.Quote;

namespace Bronto.WebApi.Services.Interfaces
{
    public interface IQuoteService
    {
        Task<QuoteResult> GetQuote(string symbol);
    }
}
