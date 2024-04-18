using Bronto.Wasm.Pwa;
using Bronto.Wasm.Pwa.Service;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient<StockService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7085/");
});

await builder.Build().RunAsync();