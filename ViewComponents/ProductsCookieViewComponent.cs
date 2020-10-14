using figma.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace figma.ViewComponents
{
    public class ProductsCookieViewComponent : ViewComponent
    {
        private readonly UnitOfWork _unitOfWork;

        public ProductsCookieViewComponent(UnitOfWork context)
        {
            _unitOfWork = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.view = HttpContext.Request.Cookies.FirstOrDefault(a => a.Key.Contains("viewProducts")).Value;
            var items = await _unitOfWork.ProductRepository.GetAync(records: 6);
            return View(items);
        }
    }
}
