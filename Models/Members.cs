using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class Members
    {
        [Key]
        [Index]
        public int MemberId { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Tài khoản")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        public string Email { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        //[MaxLength(60)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Tên đầy đủ")]
        [MaxLength(50)]
        public string Fullname { get; set; }


        [Display(Name = "Địa chỉ")]
        [MaxLength(200)]
        public string Address { get; set; }


        [Display(Name = "Số điện thoại")]
        public string Mobile { get; set; }


        [DataType(DataType.DateTime)]
        [Display(Name = "Ngày tạo")]
        public DateTime CreateDate { get; set; }


        [MaxLength(200)]
        public string HomePage { get; set; }


        [Display(Name = "Trạng thái")]
        public bool Active { get; set; }

        public virtual List<ProductLike> ProductLikes { get; set; }


    }
}
