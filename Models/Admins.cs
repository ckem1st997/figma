using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace figma.Models
{
    public class Admins
    {
        [Key]
        [Index]
        public int AdminId { get; set; }

        
        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name ="Tài khoản")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        //  [MaxLength(60)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name ="Trạng thái")]
        public bool Active { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        public int Role { get; set; }
    }
}
