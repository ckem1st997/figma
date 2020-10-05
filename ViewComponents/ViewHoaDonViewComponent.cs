using figma.DAL;
using figma.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
            var order = _unitOfWork.OrderRepository.Get(a=>a.Id==orderId, includeProperties: "OrderDetails");
            if (order == null)
            {
                return null;
            }
            return View(order);
        }
    }
}
