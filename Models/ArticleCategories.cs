using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class ArticleCategories
    {
        [Key]
        [Index]
        public int ArticleCategorieID { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Tên danh mục")]
        [MaxLength(50)]
        public string CategoryName { get; set; }


        [MaxLength(500)]

        [Display(Name = "Đường dẫn")]
        public string Link { get; set; }


        [Display(Name = "Thứ tự")]
        public int CategorySort { get; set; }


        [Display(Name = "Hoạt động")]
        public bool CategoryActive { get; set; }


        public int ParentId { get; set; }


        [Display(Name = "Hiển thị trang chủ")]
        public bool ShowHome { get; set; }


        [Display(Name = "Hiển thị Menu")]
        public bool ShowMenu { get; set; }


        [Display(Name = "Đường dẫn")]
        [MaxLength(100)]
        public string Slug { get; set; }


        [Display(Name ="")]
        public bool Hot { get; set; }


        [Display(Name ="Thẻ tiêu đề")]
        [MaxLength(100)]
        public string TitleMeta { get; set; }


        [Display(Name ="Thẻ mô tả")]
        [MaxLength(500)]
        public string DescriptionMeta { get; set; }

        public virtual ICollection<Articles> Articles { get; set; }


    }
}
