using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class ProductSizeColor
    {
        [Index]
        public int Id { get; set; }

        [Required(ErrorMessage = "Chưa nhập thông tin")]
        public int ProductID { get; set; }

        public int ColorID { get; set; }

        public int SizeID { get; set; }

        public virtual Color Color { get; set; }
        public virtual Size Size { get; set; }
        public virtual Products Products { get; set; }

    }
}
