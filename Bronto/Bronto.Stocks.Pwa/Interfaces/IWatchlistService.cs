using Bronto.Models;

namespace Bronto.Stocks.Pwa.Interfaces
{
    public interface IWatchlistService
    {
        IReadOnlyList<Stock> Stocks { get; }

        void AddStock(string stock);

        void AddStock(Stock stock);

        void RemoveStock(Stock stock);

        bool StockExists(string stockSymbol);

        void ClearPortfolio();
    }
}