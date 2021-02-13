using figma.DAL;
using figma.Models;
using figma.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace figma.ViewComponents
{
    public class ListProductsLikeViewComponent : ViewComponent
    {
        private readonly UnitOfWork _unitOfWork;

        public ListProductsLikeViewComponent(UnitOfWork context)
        {
            _unitOfWork = context;
        }

        [Authorize]
        public async Task<IViewComponentResult> InvokeAsync()
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
            Console.WriteLine(ColumIds.Count());
            return View(ColumIds);
        }
    }
}
