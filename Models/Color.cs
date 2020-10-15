
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace figma.Models
{
    public class Color
    {
        [Key]
        [Index]
        public int ColorID { get; set; }


        [Display(Name ="Mã màu")]
        public string Code { get; set; }


        [Display(Name ="Tên màu")]
        public string NameColor { get; set; }

        public virtual List<ProductSizeColor> ProductSizeColors { get; set; }
    }
}
