using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class Tags
    {
        [Key]
        [Required]
        [Index]
        public int TagID { get; set; }


        [StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), Display(Name = "Tên nhóm sản phẩm"), Required(ErrorMessage = "Hãy nhập nhóm sản phẩm"), UIHint("TextBox")]
        public string Name { get; set; }


        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Soft { get; set; }


        [Required]
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }

        public virtual List<TagProducts> TagProducts { get; set; }
    }
}
