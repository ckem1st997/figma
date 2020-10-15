using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace figma.Models
{

    public partial class Abouts
    {
        [Key]
        [Index]
        public int AboutID { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [StringLength(100)]
        [Display(Name = "Hướng dẫn và chính sách")]
        public string Subject { get; set; }


        [StringLength(500)]
        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }


        [StringLength(500)]
        [Display(Name = "Ảnh bìa")]
        public string CoverImage { get; set; }


        [Display(Name = "Nội dung")]
        public string Body { get; set; }


        [Display(Name = "Thứ hạng sắp xếp")]
        public int Sort { get; set; }


        [Display(Name = "Trạng thái")]
        public bool Active { get; set; }
    }
}
