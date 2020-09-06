using figma.DAL;
using figma.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace figma.ViewComponents
{
    public class HeaderListViewComponent : ViewComponent
    {
        private readonly UnitOfWork _unitOfWork;

        public HeaderListViewComponent(UnitOfWork context)
        {
            _unitOfWork = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = new HeaderViewModel()
            {
                ConfigSites = await _unitOfWork.ConfigSiteRepository.GetAync(),
                Abouts = await _unitOfWork.AboutRepository.GetAync(),
                ArticleCategories = await _unitOfWork.ArticleCategoryRepository.GetAync(),
                ProductCategories = await _unitOfWork.ProductCategoryRepository.GetAync()
            };
            return View(items);
        }
    }
}
