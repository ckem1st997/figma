using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class Albums
    {
        [Key]
        [Index]
        public int AlbumID { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name ="Tên Album")]
        [MaxLength(100)]
        public string Name { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        [Display(Name = "Danh sách hình ảnh")]
        public string ListImage { get; set; }


        [Display(Name = "Tiêu đề")]
        [MaxLength(100)]
        public string Title { get; set; }


        [Display(Name ="Miêu tả")]
        [MaxLength(500)]
        public string Description { get; set; }


        [Display(Name ="Soạn thảo")]
        public string Body { get; set; }


        [Display(Name ="Thứ tự hiển thị")]
        public int Sort { get; set; }


        [Display(Name = "Hiển thị lên trang chủ")]
        public bool Home { get; set; }


        [Display(Name = "Trạng thái")]
        public bool Active { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }
    }
}
