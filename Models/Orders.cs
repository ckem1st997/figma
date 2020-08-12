using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class Orders
    {

        [Index]
        public int OrdersID { get; set; }


        [Display(Name = "Mã đơn hàng")]
        [MaxLength(50)]
        public string MaDonHang { get; set; }


        [Display(Name = "Ngày tạo")]
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Thanh toán")]
        public bool Payment { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Hình thức thanh toán")]
        public int TypePay { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Hình thức vận chuyển")]
        public int Transport { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Ngày giao hàng")]
        public DateTime TransportDate { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Trạng thái")]
        public int Status { get; set; }


        [Display(Name = "")]
        public int OrderMemberId { get; set; }


        [Required]
        public bool Viewed { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name ="Họ và tên")]
        [MaxLength(50)]
        public string CustomerInfo_Fullname { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Địa chỉ")]
        [MaxLength(200)]
        public string CustomerInfo_Address { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Số điện thoại")]
        [MaxLength(11)]
        public string CustomerInfo_Mobile { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        public string CustomerInfo_Email { get; set; }


        [Display(Name = "Yêu cầu thêm")]
        [MaxLength(200)]
        public string CustomerInfo_Body { get; set; }


        [Display(Name = "Giới tính")]
        [MaxLength(10)]
        public string CustomerInfo_Gender { get; set; }


        [Display(Name ="Thanh toán trước")]
        public int ThanhToanTruoc { get; set; }


        public virtual List<OrderDetails> OrderDetails { get; set; }



    }
}
