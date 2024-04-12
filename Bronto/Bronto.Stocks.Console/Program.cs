using Bronto.Stocks.Console;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration config = builder.Build();

string keyString = config.GetSection("AppSettings")["Key"];
string hostString = config.GetSection("AppSettings")["Host"];
string subString = config.GetSection("AppSettings")["Subscription"];

// Basic Free 800 api credit limit with 8 WebSockets cedits
// 1 credit * 3 symbols equals 3 credits
// Income_statement is @ 3 symbols equals 300 credits
string uriString = string.Empty;

if (!string.IsNullOrEmpty(subString) && subString == "Basic")
{
    uriString = $"https://{hostString}/time_series?symbol=AAPL&interval=1min&format=json&apikey={keyString}";
}
else
{
    uriString = $"https://{hostString}/stocks?symbol=AAPL&exchange=NASDAQ&format=json";
}

var client = new HttpClient();

if (!string.IsNullOrEmpty(keyString) && !string.IsNullOrEmpty(hostString))
{
    var request = new HttpRequestMessage
    {
        Method = HttpMethod.Get,
        RequestUri = new Uri(uriString),
        Headers =
        {
            { "X-RapidAPI-Key", keyString },
            { "X-RapidAPI-Host", hostString },
        },
    };

    using (var response = await client.SendAsync(request))
    {
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        Console.WriteLine(body);
        
        // Time series
        var timeSeries = JsonSerializer.Deserialize<TimeSeries>(body);
        if (timeSeries.status == "ok")
        {
            Console.WriteLine("Received symbol: " + timeSeries.meta["symbol"] + ", close: " + timeSeries.values[0]["close"]);
        }
    }
}
else
{
    Console.WriteLine("Program exited without execution.");
}