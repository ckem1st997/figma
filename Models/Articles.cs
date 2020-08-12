using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class Articles
    {
        [Key]
        [Index]
        public int ArticleID { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Tên bài viết")]
        [MaxLength(100)]
        public string Subject { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Nội dung")]
        [MaxLength(500)]
        public string Description { get; set; }


        [Display(Name ="Mã Html")]
        public string Body { get; set; }


        [Display(Name ="Hình ảnh")]
        public string Image { get; set; }


        [DataType(DataType.DateTime)]
        [Display(Name ="Ngày tạo")]
        public DateTime CreateDate { get; set; }


        [Display(Name = "Lượt xem")]
        public int View { get; set; }


        public int ArticleCategorieID { get; set; }


        [Display(Name ="Kích hoạt")]
        public bool Active { get; set; }


        [Display(Name = "")]
        public bool Hot { get; set; }


        [Display(Name ="Hiển thị trang chủ")]
        public bool Home { get; set; }


        [Display(Name = "Dường dẫn")]
        [MaxLength(300)]
        [DataType(DataType.Url)]
        public string Url { get; set; }


        [Display(Name = "Tiêu đề")]
        [MaxLength(100)]
        public string TitleMeta { get; set; }


        [Display(Name = "Miêu tả")]
        [MaxLength(500)]
        public string DescriptionMeta { get; set; }

        public ArticleCategories ArticleCategories { get; set; }



    }
}
