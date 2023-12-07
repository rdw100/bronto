using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bronto.Models.Api.Price.Response
{
    public class RealTimePrice
    {
        [JsonProperty("price")]
        public string Price { get; set; }
    }
}
