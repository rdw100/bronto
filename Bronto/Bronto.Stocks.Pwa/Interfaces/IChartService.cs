using Bronto.Models;

namespace Bronto.Stocks.Pwa.Interfaces
{
    public interface IChartService
    {
        Task<List<MyOHLC>> GetStockData(
            string symbol
        );
    }
}
