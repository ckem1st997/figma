using figma.DAL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace figma.ViewComponents
{
    public class TinTucListViewComponent : ViewComponent
    {
        private readonly UnitOfWork _unitOfWork;

        public TinTucListViewComponent(UnitOfWork context)
        {
            _unitOfWork = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _unitOfWork.ArticleRepository.GetAync(a => a.Active && a.Home && a.ArticleCategory.CategoryName.Contains("Tin tức"), q => q.OrderBy(a => a.CreateDate), records: 3);
            return View(items);
        }
    }
}
