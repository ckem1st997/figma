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


        [Required]
        [Display(Name = "Thể loại")]
        [MaxLength(50)]
        public string Name { get; set; }


        [Display(Name = "Hình ảnh")]
        [MaxLength(500)]
        public string Image { get; set; }


        [Display(Name = "")]
        [MaxLength(500)]
        public string CoverImage { get; set; }


        [Display(Name = "Đường dẫn")]
        [MaxLength(500)]
        [DataType(DataType.Url)]
        public string Url { get; set; }


        public int Soft { get; set; }


        public bool Active { get; set; }


        [Display(Name = "Hiển thị trang chủ")]
        public bool Home { get; set; }


        public int ParentId { get; set; }


        [Display(Name = "Tiêu đề")]
        [MaxLength(100)]
        public string TitleMeta { get; set; }


        [Display(Name = "Miêu tả")]
        public string DescriptionMeta { get; set; }


        [Display(Name = "Mã Html")]
        public string Body { get; set; }

        public virtual List<Products> Products { get; set; }

    }
}
