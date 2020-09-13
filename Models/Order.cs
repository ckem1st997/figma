using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class Order
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string MaDonHang { get; set; }
        public DateTime CreateDate { get; set; }
        [Display(Name = "Tình trang thanh toán")]
        public bool Payment { get; set; }
        [Display(Name = "Thanh toán"), UIHint("DisplayTypePay")]
        public int TypePay { get; set; }
        [Display(Name = "Chuyển hàng"), UIHint("DisplayTypeTransport")]
        public int Transport { get; set; }
        [Display(Name = "Ngày giao hàng"), UIHint("DateTimePicker"), Required(ErrorMessage = "Hãy chọn ngày giao hàng")]
        public DateTime TransportDate { get; set; }
        [Display(Name = "Trạng thái đơn hàng"), UIHint("DisplayOrderStatus")]
        public int Status { get; set; }
        public int? OrderMemberId { get; set; }
        public bool Viewed { get; set; }
        [Display(Name = "Thanh toán trước")]
        public int ThanhToanTruoc { get; set; }
        [Display(Name = "Họ và tên *"), Required(ErrorMessage = "Hãy nhập họ và tên"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("TextBox")]
        public string Fullname { get; set; }
        [Display(Name = "Địa chỉ *"), Required(ErrorMessage = "Hãy nhập địa chỉ"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("TextBox")]
        public string Address { get; set; }
        [Display(Name = "Điện thoại *"), Required(ErrorMessage = "Hãy nhập điện thoại"), StringLength(11, MinimumLength = 7, ErrorMessage = "Điện thoại từ 7, 11 ký tự"), UIHint("TextBox")]
        public string Mobile { get; set; }
        [EmailAddress(ErrorMessage = "Email không hợp lệ"), Display(Name = "Email *"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), Required(ErrorMessage = "Hãy nhập email"), UIHint("TextBox")]
        public string Email { get; set; }
        [Display(Name = "Yêu cầu thêm"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), DataType(DataType.MultilineText)]
        public string Body { get; set; }
        [Display(Name = "Giới tính"), StringLength(10)]
        public string Gender { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public Order()
        {
            CreateDate = DateTime.Now;
            Payment = false;
            TypePay = 1;
            Viewed = false;
        }
    }

}
