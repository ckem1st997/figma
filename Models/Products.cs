using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class Products
    {
        [Index]
        public int ProductID { get; set; }


        [Required(ErrorMessage = "Xin vui lòng nhập tên sản phẩm !")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Xin vui lòng nhập mã sản phẩm !")]
        [Display(Name = "Mã sản phẩm")]
        public string Description { get; set; }


        [Required(ErrorMessage ="Xin vui lòng chọn hình ảnh !")]
        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }


        [Display(Name = "Nội dung ngắn")]
        public string Body { get; set; }


        [Display(Name = "Danh mục sản phẩm")]
        public int ProductCategorieID { get; set; }

        
        [Required(ErrorMessage ="Bạn chưa nhập thông tin")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }


        [Display(Name = "Xuất xứ")]
        [MaxLength(500)]
        public string Factory { get; set; }


        [Required(ErrorMessage = "Bạn chưa nhập thông tin")]
        [Display(Name = "Giá")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Price { get; set; }



        [Required(ErrorMessage = "Bạn chưa nhập thông tin")]
        [Display(Name = "Giá khuyến mãi")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 0)")]
        public decimal SaleOff { get; set; }


        [Display(Name ="Quy cách")]
        [MaxLength(500)]
        public string QuyCach { get; set; }


        [Required(ErrorMessage = "Bạn chưa nhập thông tin")]
        [Display(Name = "Thứ tự hiển thị")]
        public int Sort { get; set; }


        [Display(Name = "Bộ sưu tập mới nhất")]
        public bool Hot { get; set; }


        [Display(Name = "Hiển trang chủ")]
        public bool Home { get; set; }


        [Display(Name ="Hoạt động")]
        public bool Active { get; set; }


        [Display(Name = "Thẻ tiêu đề")]
        [MaxLength(100)]
        public string TitleMeta { get; set; }


        [Display(Name = "Thẻ mô tả")]
        public string DescriptionMeta { get; set; }


        [Display(Name = "Quà tặng")]
        public string GiftInfo { get; set; }


        [Display(Name = "Nội dung chi tiết")]
        public string Content { get; set; }


        [Display(Name ="Tình trạng hàng")]
        public bool StatusProduct { get; set; }


        [Display(Name = "Bộ sưu tập")]
        public int CollectionID { get; set; }


        [Display(Name = "Mã vạch")]
        [MaxLength(50)]
        public string BarCode { get; set; }


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Ngày tạo")]
        public DateTime CreateDate { get; set; }


        [Display(Name = "Người tạo")]
        public string CreateBy { get; set; }

        public virtual List<TagProducts> TagProducts { get; set; }

        public virtual List<OrderDetails> OrderDetails { get; set; }

        //public virtual List<ProductSizeColor> ProductSizeColors { get; set; }

        public virtual List<Carts> Carts { get; set; }

        public virtual List<ProductLike> ProductLikes { get; set; }

        public virtual List<ReviewProduct> ReviewProducts { get; set; }
        public virtual Collection Collection { get; set; }
        public virtual ProductCategories ProductCategories { get; set; }

    }
}
