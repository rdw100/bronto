using Bronto.Models;
using Bronto.Models.Api.Chart;

namespace Bronto.Stocks.Pwa.Interfaces
{
    public interface IChartService
    {
        Task<List<MyOHLC>> GetStockData(
            string symbol
        );

        Task<ChartResult> GetChartData(
            string symbol,
            string interval = "1d",
            string range = "5d"
        );
    }
}
