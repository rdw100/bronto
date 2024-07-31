using Bronto.Models.Api.Quote;
using Bronto.Stocks.Pwa.Interfaces;
using System.Net.Http.Json;
using static Bronto.Models.Api.Enums;

namespace Bronto.Stocks.Pwa.Services
{
    /// <summary>
    /// Accesses service to retrieve key finance data for specified stock symbol(s).
    /// </summary>
    public class QuoteService : IQuoteService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// List of stock symbols to be used for fetching stock quotes.
        /// </summary>
        /// <remarks>
        /// Each symbol represents a publicly traded company (e.g., "AAPL" for Apple Inc., "MSFT" for Microsoft Corporation).
        /// </remarks>
        public List<string> Symbols { get; } = new()
        {
            "AAPL", "ABNB", "ABMD", "ACGL", "ADBE", "ADI", "ADP", "ADSK", "AEP", "ALGN", "ALXN", "AMAT", "AMGN", "AMZN", "ANSS", "ASML", "ATVI", "AVGO", "BIDU", "BIIB", "BKNG", "BMRN", "CDNS", "CDW", "CERN", "CHKP", "CHTR", "CMCSA", "COST", "CPRT", "CSCO", "CSGP", "CSX", "CTAS", "CTSH", "CTXS", "DOCU", "DXCM", "EA", "EBAY", "EXC", "FAST", "FB", "FISV", "FOX", "FOXA", "GILD", "GOOG", "GOOGL", "IDXX", "ILMN", "INCY", "INTC", "INTU", "ISRG", "JD", "KHC", "KLAC", "LRCX", "LULU", "LUMN", "MAR", "MCHP", "MDLZ", "MELI", "MNST", "MRNA", "MRVL", "MSFT", "MU", "MXIM", "NFLX", "NTES", "NVDA", "NXPI", "OKTA", "ORLY", "PAYX", "PCAR", "PDD", "PEP", "PTON", "PYPL", "QCOM", "REGN", "ROST", "SBUX", "SGEN", "SIRI", "SNPS", "SPLK", "SWKS", "TCOM", "TEAM", "TSLA", "TXN", "VRSK", "VRSN", "VRTX", "WBA", "WDAY", "XEL", "XLNX", "ZM"
        };

        /// <summary>
        /// Initializes http to accesses service to retrieve key finance data..
        /// </summary>
        /// <param name="httpClient">An instance of HttpClient used to send HTTP requests and receive HTTP responses.</param>
        public QuoteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves key finance data for specified stock symbol(s).
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns>Returns key finance data for specified stock symbol(s)</returns>
        public async Task<QuoteResult> GetQuote(string symbol)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Quote?symbol={symbol}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<QuoteResult>();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    // Handle 429 Too Many Requests
                    return new QuoteResult
                    {
                        StatusMessage = "Rate limit exceeded. Please wait a while before making more requests.",
                        StatusCodeType = StockDataClientResponseStatus.RateLimitExceeded
                    };
                }
                else
                {
                    return new QuoteResult
                    {
                        StatusMessage = $"Stock API Error: {response.StatusCode}",
                        StatusCodeType = StockDataClientResponseStatus.StockDataApiError
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                return new QuoteResult
                {
                    StatusMessage = $"HTTP Error: {ex.Message}",
                    StatusCodeType = StockDataClientResponseStatus.StockDataError
                };
            }
        }
    }
}