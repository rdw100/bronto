using Bronto.Models.Api;

namespace Bronto.Stocks.Pwa.Interfaces
{
    public interface IPriceService
    {
        public List<string> Symbols { get; }
        Task<RealTimePrice> GetPriceAsync(string symbol);
    }
}