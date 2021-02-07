using figma.DAL;
using figma.Models;
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
            List<Products> viewProductsCookieView = new List<Products>();
            try
            {
                var listId = HttpContext.Request.Cookies.FirstOrDefault(a => a.Key.Contains("viewProducts")).Value.Split(',');
                for (int i = listId.Length - 1; i >= (listId.Length - 10 >= 0 ? listId.Length - 10 : 0); i--)
                {
                    if (listId[i].Length > 0)
                        viewProductsCookieView.Add(_unitOfWork.ProductRepository.GetByID(int.Parse(listId[i])));
                }
            }
            catch (Exception)
            {
            }
            return View(viewProductsCookieView);
        }
    }
}
