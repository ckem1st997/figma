using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    //liên hệ
    public class Contacts
    {
        [Key]
        [Index]
        public int ContactID { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name ="Họ và tên")]
        [MaxLength(50)]
        public string Fullname { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Địa chỉ")]
        [MaxLength(300)]
        public string Address { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Số điện thoại")]
        public long Mobile { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name ="Nội dung")]
        [MaxLength(100)]
        public string Subject { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [MaxLength(4000)]
        public string Body { get; set; }


        [Display(Name ="Ngày tạo")]
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }


    }
}
