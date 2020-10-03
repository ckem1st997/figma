using figma.DAL;
using figma.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace figma.ViewComponents
{
    public class HeaderListViewComponent : ViewComponent
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public HeaderListViewComponent(UnitOfWork context, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var items = new HeaderViewModel()
            {
                ConfigSites = await _unitOfWork.ConfigSiteRepository.GetAync(),
                Abouts = await _unitOfWork.AboutRepository.GetAync(),
                ArticleCategories = await _unitOfWork.ArticleCategoryRepository.GetAync(),
                ProductCategories = await _unitOfWork.ProductCategoryRepository.GetAync(a => a.Active && a.Home, q => q.OrderBy(a => a.Soft)),
                Carts = await _unitOfWork.CartRepository.GetAync(a => a.CartID == _httpContextAccessor.HttpContext.Request.Cookies["CartID"])
            };
            return View(items);
        }
    }
}
