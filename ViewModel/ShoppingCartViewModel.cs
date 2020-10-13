using figma.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace figma.ViewModel
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<Carts> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }

    public class CheckOutCompleteViewModel
    {
        public string OrderID { get; set; }
        public Contacts Contact { get; set; }
    }

    public class ShoppingCartRemoveViewModel
    {
        public string Message { get; set; }
        public decimal CartTotal { get; set; }
        public int CartCount { get; set; }
        public int Status { get; set; }
        public int DeleteId { get; set; }
    }

    public class CheckOutViewModel
    {
        
        public Order Order { get; set; }
        public decimal CartTotal { get; set; }
        [Display(Name = "Hình thức vận chuyển")]
        public int Transport { get; set; }
        [Display(Name = "Hình thức thanh toán")]
        public int TypePay { get; set; }
        public string Gender { get; set; }
        public SelectList SelectTransport { get; set; }
        public SelectList SelectTypePay { get; set; }
        public SelectList SelectGender { get; set; }
        public IEnumerable<Carts> Carts { get; set; }

        public CheckOutViewModel()
        {
            var selectTransport = new Dictionary<int, string> { { 1, "Đến địa chỉ người nhận" }, { 2, "Khách đến nhận hàng" }, { 3, "Qua bưu điện" }, { 4, "Hình thức khác" } };
            var typePay = new Dictionary<int, string> { { 1, "Tiền mặt" }, { 2, "Chuyển khoản" }, { 3, "Hình thức khác" } };
            var gender = new Dictionary<string, string> { { "Nam", "Nam" }, { "Nữ", "Nữ" } };
            SelectTransport = new SelectList(selectTransport, "Key", "Value");
            SelectTypePay = new SelectList(typePay, "Key", "Value");
            SelectGender = new SelectList(gender, "Key", "Value");
        }
    }

    public class CartSummaryViewModel
    {
        public List<Carts> Carts { get; set; }
        public decimal TotalMoney { get; set; }
        public int Count { get; set; }
    }
}
