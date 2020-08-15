﻿using System;
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
        [Display(Name = "Tiêu đề")]
        [MaxLength(100)]
        public string Subject { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Trích dẫn ngắn")]
        [MaxLength(500)]
        public string Description { get; set; }


        [Display(Name = "Nội dung")]
        public string Body { get; set; }


        [Display(Name = "Hình ảnh đại diện")]
        public string Image { get; set; }


        [DataType(DataType.DateTime)]
        [Display(Name = "Ngày tạo")]
        public DateTime CreateDate { get; set; }


        [Display(Name = "Lượt xem")]
        public int View { get; set; }


        [Required]
        [Display(Name = "Danh mục bài viết")]
        public int ArticleCategorieID { get; set; }


        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }


        [Display(Name = "")]
        public bool Hot { get; set; }


        [Display(Name = "Hiển thị trang chủ")]
        public bool Home { get; set; }


        [Display(Name = "Đường dẫn")]
        [MaxLength(300)]
        [DataType(DataType.Url)]
        public string Url { get; set; }


        [Display(Name = "Thẻ tiêu đề")]
        [MaxLength(100)]
        public string TitleMeta { get; set; }


        [Display(Name = "Thẻ mô tả")]
        [MaxLength(500)]
        public string DescriptionMeta { get; set; }

        public virtual ArticleCategories ArticleCategories { get; set; }



    }
}
