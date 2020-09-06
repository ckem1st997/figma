using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    // mạng xã hội
    public class ConfigSites
    {
        [Key]
        [Index]
        public int ConfigSiteID { get; set; }

        [Display(Name ="Tên Facebook")]
        public string Facebook { get; set; }

        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Facebook"), Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Linkedin { get; set; }


        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Tên INSTAGRAM"), UIHint("TextBox")]
        public string Youtube { get; set; }


        [StringLength(4000, ErrorMessage = "Tối đa 4000 ký tự"), Display(Name = "Đường dẫn INSTAGRAM"), UIHint("TextArea")]
        public string GoogleAnalytics { get; set; }



        [StringLength(4000, ErrorMessage = "Tối đa 4000 ký tự"), Display(Name = "Đường dẫn Shopee"), UIHint("TextArea")]
        public string LiveChat { get; set; }
        

        [StringLength(100, ErrorMessage = "Tối đa 4000 ký tự"), Display(Name = "Tên Shopee"), UIHint("TextArea")]
        public string nameShopee { get; set; }

        [StringLength(100, ErrorMessage = "Tối đa 4000 ký tự"), Display(Name = "Trang Web"), UIHint("TextArea")]
        public string urlWeb { get; set; }


        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Google Plus"), Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string GooglePlus { get; set; }


        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Twitter"), Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Twitter { get; set; }


        [Display(Name = "Mã nhúng Bản đồ Google map"), StringLength(4000, ErrorMessage = "Tối đa 4000 ký tự"), UIHint("TextArea")]
        public string GoogleMap { get; set; }


        [Display(Name = "Thẻ title"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("TextBox")]
        public string Title { get; set; }


        [Display(Name = "Thẻ description"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string Description { get; set; }


        [Display(Name = "Thông tin liên hệ"), UIHint("EditorBox")]
        public string ContactInfo { get; set; }


        [Display(Name = "Đường dẫn chân trang"), UIHint("EditorBox")]
        public string FooterInfo { get; set; }


        [Display(Name = "Hotline"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("TextBox")]
        public string Hotline { get; set; }


        [Display(Name = "Email"), EmailAddress(ErrorMessage = "Email không hợp lệ"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string Email { get; set; }


        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Logo")]
        public string CoverImage { get; set; }


        [Display(Name = "Chương trình khuyến mãi"), UIHint("EditorBox")]
        public string SaleOffProgram { get; set; }




    }
}
