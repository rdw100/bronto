using Bronto.Models;

namespace Bronto.Stocks.Pwa.Interfaces
{
    public interface IPortfolioService
    {
        IReadOnlyList<Stock> Stocks { get; }

        void AddStock(Stock stock);

        void RemoveStock(Stock stock);

        bool StockExists(string stockSymbol);

        void ClearPortfolio();
    }
}