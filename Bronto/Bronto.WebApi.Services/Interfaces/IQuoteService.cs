using Bronto.Models.Api.Quote;

namespace Bronto.WebApi.Services.Interfaces
{
    /// <summary>
    /// Defines stock quote data service access
    /// </summary>
    public interface IQuoteService
    {        
        Task<QuoteResult> GetQuote(string symbol);
    }
}