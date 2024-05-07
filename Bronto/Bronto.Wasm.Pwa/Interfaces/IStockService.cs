using Bronto.Models.Api;

namespace Bronto.Wasm.Pwa.Interfaces
{
    public interface IStockService
    {
        Task<StockDataTimeSeries> GetTimeSeriesAsync(string symbol, string interval, string outputsize);
    }
}