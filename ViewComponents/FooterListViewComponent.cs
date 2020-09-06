using figma.DAL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace figma.ViewComponents
{
    public class FooterListViewComponent : ViewComponent
    {
        private readonly UnitOfWork _unitOfWork;

        public FooterListViewComponent(UnitOfWork context)
        {
            _unitOfWork = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _unitOfWork.ConfigSiteRepository.GetAync();
            return View(items);
        }
    }
}
