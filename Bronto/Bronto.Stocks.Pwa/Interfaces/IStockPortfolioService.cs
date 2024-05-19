using Bronto.Models;

namespace Bronto.Stocks.Pwa.Interfaces
{
    public interface IStockPortfolioService
    {
        IReadOnlyList<Stock> Stocks { get; }

        void AddStock(Stock stock);

        void RemoveStock(string stockSymbol);

        void ClearPortfolio();
    }
}