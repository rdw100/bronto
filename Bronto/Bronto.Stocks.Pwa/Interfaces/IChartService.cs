using Bronto.Models;
using Bronto.Models.Api.Chart;
using Bronto.Models.Enums;

namespace Bronto.Stocks.Pwa.Interfaces
{
    public interface IChartService
    {
        Task<ChartResult> GetChartData(
            string symbol,
            StockInterval interval = StockInterval.OneDay,
            StockRange range = StockRange.FiveDays
        );
    }
}
