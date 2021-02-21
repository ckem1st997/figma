using figma.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace figma.ViewComponents
{
    [Authorize(Roles = "Admin")]
    public class ViewHoaDonViewComponent : ViewComponent
    {
        private readonly UnitOfWork _unitOfWork;

        public ViewHoaDonViewComponent(UnitOfWork context)
        {
            _unitOfWork = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetAync(a => a.Id == orderId, includeProperties: "OrderDetails");
            ViewBag.pro = _unitOfWork.ProductRepository.Get().Select(a => a.ProductID);
            try
            {
                ViewBag.use = _unitOfWork.UserVoucherRepository.Get(x => x.MaDonHang.Equals(_unitOfWork.OrderRepository.GetByID(orderId).MaDonHang)).FirstOrDefault().SumHD;
            }
            catch (System.Exception)
            {
                ViewBag.use = null;
            }
            if (order == null)
            {
                return null;
            }
            return View(order);
        }
    }
}
