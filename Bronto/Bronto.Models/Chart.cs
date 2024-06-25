namespace Bronto.Models.Api.Chart
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public partial class ChartResult
    {
        [JsonProperty("chart")]
        public Chart Chart { get; set; }
    }

    public partial class Chart
    {
        [JsonProperty("result")]
        public List<Result> Result { get; set; }

        [JsonProperty("error")]
        public object Error { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("timestamp")]
        public List<long> Timestamp { get; set; }

        [JsonProperty("indicators")]
        public Indicators Indicators { get; set; }
    }

    public partial class Indicators
    {
        [JsonProperty("quote")]
        public List<Quote> Quote { get; set; }

        [JsonProperty("adjclose")]
        public List<Adjclose> Adjclose { get; set; }
    }

    public partial class Adjclose
    {
        [JsonProperty("adjclose")]
        public List<double> AdjcloseAdjclose { get; set; }
    }

    public partial class Quote
    {
        [JsonProperty("open")]
        public List<double> Open { get; set; }

        [JsonProperty("high")]
        public List<double> High { get; set; }

        [JsonProperty("low")]
        public List<double> Low { get; set; }

        [JsonProperty("close")]
        public List<double> Close { get; set; }

        [JsonProperty("volume")]
        public List<long> Volume { get; set; }
    }

    public partial class Meta
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("exchangeName")]
        public string ExchangeName { get; set; }

        [JsonProperty("fullExchangeName")]
        public string FullExchangeName { get; set; }

        [JsonProperty("instrumentType")]
        public string InstrumentType { get; set; }

        [JsonProperty("firstTradeDate")]
        public long FirstTradeDate { get; set; }

        [JsonProperty("regularMarketTime")]
        public long RegularMarketTime { get; set; }

        [JsonProperty("hasPrePostMarketData")]
        public bool HasPrePostMarketData { get; set; }

        [JsonProperty("gmtoffset")]
        public long Gmtoffset { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("exchangeTimezoneName")]
        public string ExchangeTimezoneName { get; set; }

        [JsonProperty("regularMarketPrice")]
        public double RegularMarketPrice { get; set; }

        [JsonProperty("fiftyTwoWeekHigh")]
        public double FiftyTwoWeekHigh { get; set; }

        [JsonProperty("fiftyTwoWeekLow")]
        public double FiftyTwoWeekLow { get; set; }

        [JsonProperty("regularMarketDayHigh")]
        public double RegularMarketDayHigh { get; set; }

        [JsonProperty("regularMarketDayLow")]
        public double RegularMarketDayLow { get; set; }

        [JsonProperty("regularMarketVolume")]
        public long RegularMarketVolume { get; set; }

        [JsonProperty("chartPreviousClose")]
        public double ChartPreviousClose { get; set; }

        [JsonProperty("priceHint")]
        public long PriceHint { get; set; }

        [JsonProperty("currentTradingPeriod")]
        public CurrentTradingPeriod CurrentTradingPeriod { get; set; }

        [JsonProperty("dataGranularity")]
        public string DataGranularity { get; set; }

        [JsonProperty("range")]
        public string Range { get; set; }

        [JsonProperty("validRanges")]
        public List<string> ValidRanges { get; set; }
    }

    public partial class CurrentTradingPeriod
    {
        [JsonProperty("pre")]
        public Post Pre { get; set; }

        [JsonProperty("regular")]
        public Post Regular { get; set; }

        [JsonProperty("post")]
        public Post Post { get; set; }
    }

    public partial class Post
    {
        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("end")]
        public long End { get; set; }

        [JsonProperty("gmtoffset")]
        public long Gmtoffset { get; set; }
    }
}