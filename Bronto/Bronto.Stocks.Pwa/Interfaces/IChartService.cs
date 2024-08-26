using Bronto.Models;
using Bronto.Models.Api.Chart;
using Bronto.Models.Enums;

namespace Bronto.Stocks.Pwa.Interfaces
{
    public interface IChartService
    {
        Task<ChartResult> GetChartData(
            string symbol,
            string interval = "1d",
            StockRange range = StockRange.FiveDays
        );
    }
}
