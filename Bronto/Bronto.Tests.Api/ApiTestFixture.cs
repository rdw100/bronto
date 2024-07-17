using Microsoft.AspNetCore.Mvc.Testing;

namespace Bronto.Tests.Api
{
    public class ApiTestFixture : IDisposable
    {
        public HttpClient Client { get; private set; }

        public ApiTestFixture()
        {
            var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Additional configuration if needed
                    });
                });

            Client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost:7048/")
            });
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}