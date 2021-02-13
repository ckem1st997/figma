using figma.DAL;
using figma.Models;
using figma.ViewModel;
using Microsoft.AspNetCore.Authorization;
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

        private readonly ILogger<ApiController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly UnitOfWork _unitOfWork;
        public ApiController(ILogger<ApiController> logger, IHttpClientFactory clientFactory, UnitOfWork unitOfWork)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://thongtindoanhnghiep.co/api/city");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return Ok(await response.Content.ReadAsStringAsync());
            return Ok(false);
        }
        [HttpGet("GetHuyen/{id}")]
        public async Task<IActionResult> GetHuyen(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://thongtindoanhnghiep.co/api/city/" + id + "/district");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return Ok(await response.Content.ReadAsStringAsync());
            return Ok(false);
        }
        [HttpGet("GetXa/{id}")]
        public async Task<IActionResult> GetXa(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://thongtindoanhnghiep.co/api/district/" + id + "/ward");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return Ok(await response.Content.ReadAsStringAsync());
            return Ok(false);
        }

        [Authorize]
        [HttpGet("GetLikeProducts")]
        public IActionResult GetLikeProducts()
        {

            var claims = HttpContext.User.Claims;
            var userId = claims.FirstOrDefault(c => c.Type == "UserId").Value;
            var list = _unitOfWork.ProductLikeRepository.Get(x => x.MemberId.Equals(int.Parse(userId))).Select(x => x.ProductID);
            List<ViewProducts> ColumIds = new List<ViewProducts>();
            foreach (var item in list)
            {
                Products temp = _unitOfWork.ProductRepository.GetByID(item);
                ViewProducts products = new ViewProducts();
                products.Name = temp.Name;
                products.CreateDate = temp.CreateDate;
                products.Hot = temp.Hot;
                products.Image = temp.Image;
                products.Price = temp.Price;
                products.ProductID = temp.ProductID;
                products.SaleOff = temp.SaleOff;
                products.Sort = temp.Sort;
                products.Quantity = temp.Quantity;
                if (products != null)
                    ColumIds.Add(products);
            }
            return Ok(ColumIds);
        }

    }
}
