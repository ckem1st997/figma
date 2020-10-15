using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace figma.Models
{
    public class Collection
    {
        [Index]
        public int CollectionID { get; set; }


        [Display(Name = "Tên bộ sưu tập")]
        public string Name { get; set; }


        [Display(Name = "Mã bộ sưu tập")]
        public string Description { get; set; }


        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }


        [Display(Name = "Nội dung")]
        public string Body { get; set; }


        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }


        [Display(Name = "Xuất xứ")]
        [MaxLength(500)]
        public string Factory { get; set; }


        [Display(Name = "Giá")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Price { get; set; }


        [Display(Name ="Thứ tự hiển thị")]
        public int Sort { get; set; }


        [Display(Name = "Bộ sưu tập mới nhất")]
        public bool Hot { get; set; }


        [Display(Name = "Hiển thị trang chủ")]
        public bool Home { get; set; }


        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }


        [Display(Name = "Thẻ tiêu đề")]
        [MaxLength(100)]
        public string TitleMeta { get; set; }


        [Display(Name = "Nội dung")]
        public string Content { get; set; }


        public bool StatusProduct { get; set; }


        [Display(Name = "Mã vạch")]
        [MaxLength(50)]
        public string BarCode { get; set; }


        [DataType(DataType.DateTime)]
        [Display(Name = "Ngày tạo")]
        public DateTime CreateDate { get; set; }


        [Display(Name = "Người tạo")]
        public string CreateBy { get; set; }

        public virtual List<Products> Products { get; set; }
    }
}
