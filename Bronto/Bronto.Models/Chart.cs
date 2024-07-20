namespace Bronto.Models.Api.Chart
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class ChartResult : ApiResponse
    {
        [JsonPropertyName("chart")]
        public Chart Chart { get; set; }
    }

    public partial class Chart
    {
        [JsonPropertyName("result")]
        public List<Result> Result { get; set; }

        [JsonPropertyName("error")]
        public object Error { get; set; }
    }

    public partial class Result
    {
        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }

        [JsonPropertyName("timestamp")]
        public List<long> Timestamp { get; set; }

        [JsonPropertyName("indicators")]
        public Indicators Indicators { get; set; }
    }

    public partial class Indicators
    {
        [JsonPropertyName("quote")]
        public List<Quote> Quote { get; set; }

        [JsonPropertyName("adjclose")]
        public List<Adjclose> Adjclose { get; set; }
    }

    public partial class Adjclose
    {
        [JsonPropertyName("adjclose")]
        public List<double> AdjcloseAdjclose { get; set; }
    }

    public partial class Quote
    {
        [JsonPropertyName("open")]
        public List<double> Open { get; set; }

        [JsonPropertyName("high")]
        public List<double> High { get; set; }

        [JsonPropertyName("low")]
        public List<double> Low { get; set; }

        [JsonPropertyName("close")]
        public List<double> Close { get; set; }

        [JsonPropertyName("volume")]
        public List<long> Volume { get; set; }
    }

    public partial class Meta
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("exchangeName")]
        public string ExchangeName { get; set; }

        [JsonPropertyName("fullExchangeName")]
        public string FullExchangeName { get; set; }

        [JsonPropertyName("instrumentType")]
        public string InstrumentType { get; set; }

        [JsonPropertyName("firstTradeDate")]
        public long FirstTradeDate { get; set; }

        [JsonPropertyName("regularMarketTime")]
        public long RegularMarketTime { get; set; }

        [JsonPropertyName("hasPrePostMarketData")]
        public bool HasPrePostMarketData { get; set; }

        [JsonPropertyName("gmtoffset")]
        public long Gmtoffset { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("exchangeTimezoneName")]
        public string ExchangeTimezoneName { get; set; }

        [JsonPropertyName("regularMarketPrice")]
        public double RegularMarketPrice { get; set; }

        [JsonPropertyName("fiftyTwoWeekHigh")]
        public double FiftyTwoWeekHigh { get; set; }

        [JsonPropertyName("fiftyTwoWeekLow")]
        public double FiftyTwoWeekLow { get; set; }

        [JsonPropertyName("regularMarketDayHigh")]
        public double RegularMarketDayHigh { get; set; }

        [JsonPropertyName("regularMarketDayLow")]
        public double RegularMarketDayLow { get; set; }

        [JsonPropertyName("regularMarketVolume")]
        public long RegularMarketVolume { get; set; }

        [JsonPropertyName("chartPreviousClose")]
        public double ChartPreviousClose { get; set; }

        [JsonPropertyName("priceHint")]
        public long PriceHint { get; set; }

        [JsonPropertyName("currentTradingPeriod")]
        public CurrentTradingPeriod CurrentTradingPeriod { get; set; }

        [JsonPropertyName("dataGranularity")]
        public string DataGranularity { get; set; }

        [JsonPropertyName("range")]
        public string Range { get; set; }

        [JsonPropertyName("validRanges")]
        public List<string> ValidRanges { get; set; }
    }

    /// <summary>
    /// Calculates the price Change is the difference between the
    /// current market price and the previous close price.
    /// </summary>
    public partial class MetaPriceCalculated
    {
        public decimal CurrentPrice { get; set; }
        public decimal PreviousPrice { get; set; }
        public string PriceChangeString => PriceChange > 0 ? $"+{PriceChange:F2}" : $"{PriceChange:F2}";
        public string PercentageChangeString => PriceChange > 0 ? $"+{PercentPriceChange:F2}%" : $"{PercentPriceChange:F2}%";
        private string ChangeColor => PriceChange > 0 ? "green" : "red";

        // Calculate the price change
        public decimal PriceChange => CurrentPrice - PreviousPrice;

        // Calculate the percent price change
        public decimal PercentPriceChange
        {
            get
            {
                if (PreviousPrice != 0)
                {
                    return (PriceChange / PreviousPrice) * 100;
                }
                else
                {
                    // Handle the case when PreviousPrice is zero (avoid division by zero)
                    return 0;
                }
            }
        }
    }

    public partial class CurrentTradingPeriod
    {
        [JsonPropertyName("pre")]
        public Post Pre { get; set; }

        [JsonPropertyName("regular")]
        public Post Regular { get; set; }

        [JsonPropertyName("post")]
        public Post Post { get; set; }
    }

    public partial class Post
    {
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("start")]
        public long Start { get; set; }

        [JsonPropertyName("end")]
        public long End { get; set; }

        [JsonPropertyName("gmtoffset")]
        public long Gmtoffset { get; set; }
    }
}