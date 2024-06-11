using Bronto.Tests.RateLimit;
using System.Threading.RateLimiting;

/*
Create a client-side HTTP handler that rate limits the number of requests it sends
Client-side policy does not exceed server-side limits, server-side limit is met
first and remaining requests receive a 429. Adjust server-side limits for expected result
*/
Thread.Sleep(8000);

var options = new FixedWindowRateLimiterOptions
    {
        AutoReplenishment = true,
        PermitLimit = 8,
        Window = TimeSpan.FromSeconds(60)
    };

// Create an HTTP client with the client-side rate limited handler.
using HttpClient client = new(
    handler: new ClientSideRateLimitedHandler(
        limiter: new FixedWindowRateLimiter(options)        
        ));

// Create 8 urls with a unique query string.
var oneHundredUrls = Enumerable.Range(1, 8).Select(
    i => $"https://localhost:7085/api/Price?symbol=aapl&iteration={i:0#}");

// Flood the HTTP client with requests.
var floodOneThroughFourTask = Parallel.ForEachAsync(
    source: oneHundredUrls.Take(1..4),
    body: (url, cancellationToken) => GetAsync(client, url, cancellationToken));

var floodFiveThroughEightTask = Parallel.ForEachAsync(
    source: oneHundredUrls.Take(^5..),
    body: (url, cancellationToken) => GetAsync(client, url, cancellationToken));

await Task.WhenAll(
    floodOneThroughFourTask,
    floodFiveThroughEightTask);

Console.ReadLine();

static async ValueTask GetAsync(
    HttpClient client, string url, CancellationToken cancellationToken)
{
    using var response =
        await client.GetAsync(url, cancellationToken);

    Console.WriteLine(
        $"URL: {url}, HTTP status code: {response.StatusCode} ({(int)response.StatusCode})");
}