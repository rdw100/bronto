using Bronto.Models.Api.Quote;

namespace Bronto.Stocks.Pwa.Interfaces
{
    /// <summary>
    /// Defines stock quote front-end data & services access
    /// </summary>
    public interface IQuoteService
    {
        public List<string> Symbols { get; }
        Task<QuoteResult> GetQuote(string symbol);
    }
}
