using Bronto.Models;
using Bronto.Stocks.Pwa.Interfaces;

namespace Bronto.Stocks.Pwa.Services
{
    /// <summary>
    /// Represents a dynamic list of stocks that you’re monitoring.
    /// </summary>
    public class WatchlistService : IWatchlistService
    {
        private List<Stock> _stocks = new List<Stock>();

        public IReadOnlyList<Stock> Stocks => _stocks;

        public void AddStock(string stock)
        {
            if (!string.IsNullOrWhiteSpace(stock) && !StockExists(stock))
            {
                // Create a new stock with the entered symbol
                var newStock = new Stock
                {
                    Symbol = stock
                };

                // Add the stock to the portfolio
                _stocks.Add(newStock);
            }
        }

        public void AddStock(Stock stock)
        {
            _stocks.Add(stock);
        }

        public void RemoveStock(Stock stock)
        {
            var stockToRemove = _stocks.FirstOrDefault(s => s.Symbol == stock.Symbol);
            if (stockToRemove != null)
            {
                _stocks.Remove(stockToRemove);
            }
        }

        public bool StockExists(string symbol)
        {
            return _stocks.Exists(s => s.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));
        }

        public void ClearPortfolio()
        {
            _stocks.Clear();
        }
    }
}