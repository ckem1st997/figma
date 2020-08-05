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

        
        [Required]
        [Display(Name ="Tài khoản")]
        public string Username { get; set; }


        [Required]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        //  [MaxLength(60)]
        public string Password { get; set; }


        [Required]
        [Display(Name ="Trạng thái")]
        public bool Active { get; set; }


        [Required]
        public int Role { get; set; }
    }
}
