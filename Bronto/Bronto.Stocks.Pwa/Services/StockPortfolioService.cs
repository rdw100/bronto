using Bronto.Models;
using Bronto.Stocks.Pwa.Interfaces;

namespace Bronto.Stocks.Pwa.Services
{
    public class StockPortfolioService : IStockPortfolioService
    {
        private List<Stock> _stocks = new List<Stock>();

        public IReadOnlyList<Stock> Stocks => _stocks;

        public void AddStock(Stock stock)
        {
            _stocks.Add(stock);
        }

        public void RemoveStock(string stockSymbol)
        {
            var stockToRemove = _stocks.FirstOrDefault(s => s.Symbol == stockSymbol);
            if (stockToRemove != null)
            {
                _stocks.Remove(stockToRemove);
            }
        }
        
        public void ClearPortfolio()
        {
            _stocks.Clear();
        }
    } 
}