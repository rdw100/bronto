using Bronto.Models.Api;
using Bronto.Stocks.Pwa.Interfaces;
using System.Net.Http.Json;
using static Bronto.Models.Api.Enums;

namespace Bronto.Stocks.Pwa.Services
{
    public class PriceService : IPriceService
    {
        private readonly HttpClient _httpClient;

        public List<string> Symbols { get; } = new()
        {
            "AAPL", "ABNB", "ABMD", "ACGL", "ADBE", "ADI", "ADP", "ADSK", "AEP", "ALGN", "ALXN", "AMAT", "AMGN", "AMZN", "ANSS", "ASML", "ATVI", "AVGO", "BIDU", "BIIB", "BKNG", "BMRN", "CDNS", "CDW", "CERN", "CHKP", "CHTR", "CMCSA", "COST", "CPRT", "CSCO", "CSGP", "CSX", "CTAS", "CTSH", "CTXS", "DOCU", "DXCM", "EA", "EBAY", "EXC", "FAST", "FB", "FISV", "FOX", "FOXA", "GILD", "GOOG", "GOOGL", "IDXX", "ILMN", "INCY", "INTC", "INTU", "ISRG", "JD", "KHC", "KLAC", "LRCX", "LULU", "LUMN", "MAR", "MCHP", "MDLZ", "MELI", "MNST", "MRNA", "MRVL", "MSFT", "MU", "MXIM", "NFLX", "NTES", "NVDA", "NXPI", "OKTA", "ORLY", "PAYX", "PCAR", "PDD", "PEP", "PTON", "PYPL", "QCOM", "REGN", "ROST", "SBUX", "SGEN", "SIRI", "SNPS", "SPLK", "SWKS", "TCOM", "TEAM", "TSLA", "TXN", "VRSK", "VRSN", "VRTX", "WBA", "WDAY", "XEL", "XLNX", "ZM"
        };

        public PriceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RealTimePrice> GetPriceAsync(string symbol)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Price?symbol={symbol}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RealTimePrice>();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    // Handle 429 Too Many Requests
                    return new RealTimePrice
                    {
                        ResponseMessage = "Rate limit exceeded. Please wait a while before making more requests.",
                        ResponseStatus = StockDataClientResponseStatus.RateLimitExceeded
                    };
                }
                else
                {
                    return new RealTimePrice
                    {
                        ResponseMessage = $"Stock API Error: {response.StatusCode}",
                        ResponseStatus = StockDataClientResponseStatus.StockDataApiError
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                return new RealTimePrice
                {
                    ResponseMessage = $"HTTP Error: {ex.Message}",
                    ResponseStatus = StockDataClientResponseStatus.StockDataError
                };
            }
        }
    }
}