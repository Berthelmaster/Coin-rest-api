using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BitcoinController : ControllerBase
    {
        private const string Key = "5341DEFE-3D81-40E5-AB0A-71023208BE1E";
        private readonly IHttpClientFactory _clientFactory;
        public BitcoinController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetBitcoinRates()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://rest.coinapi.io/v1/exchangerate/BTC/USD/history?period_id=4HRS&time_start=2021-01-01T00:00:00&apikey={Key}");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            var stream = await response.Content.ReadAsStreamAsync();
            var content = await JsonSerializer.DeserializeAsync<object>(stream);
            var result = new Result()
            {
                KeysLeft = response.Headers.FirstOrDefault(x => x.Key == "x-ratelimit-remaining").Value,
                Response = content
            };
            return Ok(result);
        }
        
    }

    public class Result
    {
        public object KeysLeft { get; set; }
        public object Response { get; set; }
    }
}