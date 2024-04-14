using Microsoft.AspNetCore.Mvc;

namespace Bronto.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSeriesController : ControllerBase
    {
        private readonly HttpClient _client;

        public TimeSeriesController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://myapi.stocks.org/data/1.2");
        }
    }
}
