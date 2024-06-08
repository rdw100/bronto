using Bronto.Models;
using Bronto.Stocks.Pwa.Interfaces;

namespace Bronto.Stocks.Pwa.Services
{
    /// <summary>
    /// Represents a collection of stocks that an investor holds.
    /// </summary>
    public class PortfolioService : IPortfolioService
    {
        private List<Stock> _stocks = new List<Stock>();

        public IReadOnlyList<Stock> Stocks => _stocks;

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