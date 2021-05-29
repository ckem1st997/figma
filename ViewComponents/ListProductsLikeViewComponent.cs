using figma.DAL;
using figma.Models;
using figma.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace figma.ViewComponents
{
    public class ListProductsLikeViewComponent : ViewComponent
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IDapper _dapper;

        public ListProductsLikeViewComponent(UnitOfWork context, IDapper dapper)
        {
            _unitOfWork = context;
            _dapper = dapper;
        }

        [Authorize]
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var claims = HttpContext.User.Claims;
            var userId = claims.FirstOrDefault(c => c.Type == "UserId").Value;
            // var list = _unitOfWork.ProductLikeRepository.Get(x => x.MemberId.Equals(int.Parse(userId))).Select(x => x.ProductID);
            var list = _dapper.GetAll<int>("select ProductID from ProductLikes where MemberId=3", null, CommandType.Text);
            List<ViewProducts> ColumIds = new List<ViewProducts>();
            foreach (var item in list)
            {
                //Products temp = _unitOfWork.ProductRepository.GetByID(item);
                var Temp = _dapper.Get<ViewProducts>("Select Name,CreateDate,Hot,Image,Price,ProductID,SaleOff,Sort,Quantity from Products where ProductID=" + item + "",null,CommandType.Text);
                //  ViewProducts products = new ViewProducts();
                //products.Name = temp.Name;
                //products.CreateDate = temp.CreateDate;
                //products.Hot = temp.Hot;
                //products.Image = temp.Image;
                //products.Price = temp.Price;
                //products.ProductID = temp.ProductID;
                //products.SaleOff = temp.SaleOff;
                //products.Sort = temp.Sort;
                //products.Quantity = temp.Quantity;
                //if (products != null)
                //    ColumIds.Add(products);
                if (Temp != null)
                    ColumIds.Add(Temp);
            }
            return View(ColumIds);
        }
    }
}
