using Bronto.Models;
using Bronto.Models.Api.Chart;

namespace Bronto.WebApi.Services.Interfaces
{
    public interface IChartService
    {
        Task<List<MyOHLC>> GetStockData(
            string symbol,
            string interval = "1d",
            string range = "5d",
            long? period1 = null,
            long? period2 = null
        );

        Task<ChartResult> GetChartData(
            string symbol,
            string interval = "1d",
            string range = "5d",
            long? period1 = null,
            long? period2 = null
        );
    }
}
