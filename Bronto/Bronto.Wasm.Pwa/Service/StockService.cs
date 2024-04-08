namespace Bronto.Wasm.Pwa.Service
{
    public class StockService
    {
        private readonly HttpClient _httpClient;

        public StockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetStockPriceAsync(string tickerSymbol)
        {
            // Add logic here to fetch stock price from an API
            // For this example, we'll just return a random price
            Random rnd = new Random();
            return await Task.FromResult<decimal>(rnd.Next(5000, 20000) / 100);
        }
    }
}
