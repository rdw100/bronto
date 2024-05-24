using Bronto.Models.Api;

namespace Bronto.Stocks.Pwa.Interfaces
{
    public interface IPriceService
    {
        Task<RealTimePrice> GetPriceAsync(string symbol);
    }
}