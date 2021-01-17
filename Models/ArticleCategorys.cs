using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class ArticleCategorys
    {
        [Key]
        public int ArticleCategoryId { get; set; }
        [Display(Name = "Tên danh mục"), Required(ErrorMessage = "Hãy nhập tên danh mục"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("TextBox")]
        public string CategoryName { get; set; }
        [Display(Name = "Đường dẫn"), Url(ErrorMessage = "Đường dẫn không hợp lệ"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextBox")]
        public string Url { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int CategorySort { get; set; }
        [Display(Name = "Hoạt động")]
        public bool CategoryActive { get; set; }
        [Display(Name = "Danh mục cha")]
        public int? ParentId { get; set; }
        [Display(Name = "Hiển thị trang chủ")]
        public bool ShowHome { get; set; }
        [Display(Name = "Hiển thị menu ngang")]
        public bool ShowMenu { get; set; }
        [StringLength(100), Display(Name = "Đường dẫn danh mục"), UIHint("TextBox")]
        public string Slug { get; set; }
        [Display(Name = "Danh mục Hot")]
        public bool Hot { get; set; }
        [Display(Name = "Thẻ tiêu đề"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string TitleMeta { get; set; }
        [Display(Name = "Thẻ mô tả"), UIHint("TextArea")]
        public string DescriptionMeta { get; set; }

        public virtual ICollection<Articles> Articles { get; set; }

        public ArticleCategorys()
        {
            ShowMenu = false;
            ShowHome = false;
        }
    }
}
