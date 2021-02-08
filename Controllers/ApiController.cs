using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace figma.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
       {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ApiController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        public ApiController(ILogger<ApiController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,"https://thongtindoanhnghiep.co/api/city");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return Ok(await response.Content.ReadAsStringAsync());
            return Ok(false);
        }
        [HttpGet("GetHuyen/{id}")]
        public async Task<IActionResult> GetHuyen(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,"https://thongtindoanhnghiep.co/api/city/"+id+"/district");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return Ok(await response.Content.ReadAsStringAsync());
            return Ok(false);
        }
        [HttpGet("GetXa/{id}")]
        public async Task<IActionResult> GetXa(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://thongtindoanhnghiep.co/api/district/"+id+"/ward");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return Ok(await response.Content.ReadAsStringAsync());
            return Ok(false);
        }
    }
}
