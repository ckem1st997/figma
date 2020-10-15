using figma.DAL;
using figma.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace figma.ViewComponents
{
    public class LoadOrderViewComponent : ViewComponent
    {
        private readonly UnitOfWork _unitOfWork;

        public LoadOrderViewComponent(UnitOfWork context)
        {
            _unitOfWork = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int orderId = 0)
        {
            var order = _unitOfWork.OrderRepository.GetByID(orderId);
            var orderrderdetails = await _unitOfWork.OrderDetailRepository.GetAync(a => a.OrderId == orderId, includeProperties: "Product,Order");
            var model = new OrderViewModel
            {
                Order = order,
                OrderDetails = orderrderdetails
            };
            return View(model);
        }
    }
}
