using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class Size
    {
        [Index]
        public int SizeID { get; set; }


        [MaxLength(50)]
        [Display(Name ="Size")]
        public string SizeProduct { get; set; }

        public virtual List<ProductSizeColor> ProductSizeColors { get;set; }
    }
}
