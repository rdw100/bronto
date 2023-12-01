using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration config = builder.Build();

string keyString = config.GetSection("AppSettings")["Key"];
string hostString = config.GetSection("AppSettings")["Host"];
string uriString = $"https://{hostString}/stocks?symbol=AAPL&exchange=NASDAQ&format=json";

var client = new HttpClient();

if (string.IsNullOrEmpty(keyString) || string.IsNullOrEmpty(hostString))
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
    }
}
else
{
    Console.WriteLine("Program exited without execution.");
}