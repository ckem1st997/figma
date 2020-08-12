using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class ProductCategories
    {
        [Index]
        public int ProductCategorieID { get; set; }


        [Required(ErrorMessage ="Chưa nhập thông tin")]
        [Display(Name = "Tên danh mục")]
        [MaxLength(50)]
        public string Name { get; set; }



        [Display(Name = "Ảnh banner")]
        [MaxLength(500)]
        public string Image { get; set; }


        [Display(Name = "Biểu tượng")]
        [MaxLength(500)]
        public string CoverImage { get; set; }


        [Display(Name = "Đường dẫn")]
        [MaxLength(500)]
        public string Url { get; set; }


        [Display(Name ="Thứ tự")]
        public int Soft { get; set; }


        [Display(Name ="Hoạt động")]
        public bool Active { get; set; }


        [Display(Name = "Hiển trang chủ")]
        public bool Home { get; set; }


        [Display(Name="Danh mục cha")]
        public int? ParentId { get; set; }


        [Display(Name = "Thẻ tiêu đề")]
        [MaxLength(100)]
        public string TitleMeta { get; set; }


        [Display(Name = "Thẻ mô tả")]
        public string DescriptionMeta { get; set; }


        [Display(Name = "Nội dung sản phẩm")]
        public string Body { get; set; }

        public virtual List<Products> Products { get; set; }

    }
}
