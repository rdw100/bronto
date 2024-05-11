using Bronto.Models.Api;

namespace Bronto.Stocks.Pwa.Interfaces
{
    public interface IStockService
    {
        Task<StockDataTimeSeries> GetTimeSeriesAsync(string symbol, string interval, string outputsize);
    }
}
