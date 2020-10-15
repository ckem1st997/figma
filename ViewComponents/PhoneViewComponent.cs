using figma.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace figma.ViewComponents
{
    public class PhoneViewComponent : ViewComponent
    {
        private readonly UnitOfWork _unitOfWork;

        public PhoneViewComponent(UnitOfWork context)
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
