using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class Banners
    {
        [Key]
        [Index]
        public int BannerID { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [MaxLength(100)]
        [Display(Name ="Tên banner")]
        public string BannerName { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name ="Chiều rộng banner")]
        public int Width { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Chiều dài banner")]
        public int Height { get; set; }


        [Display(Name ="Kích hoạt")]
        public bool Active { get; set; }


        public int GroupId { get; set; }


        [Display(Name = "Dường dẫn")]
        [MaxLength(500)]
        [DataType(DataType.Url)]
        public string Url { get; set; }


        [Display(Name ="Thứ tự hiển thị")]
        public int Soft { get; set; }

    }
}
