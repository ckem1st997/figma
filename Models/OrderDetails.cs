using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class OrderDetails
    {
        [Index]
        public int OrdersID { get; set; }

        public virtual Orders Orders { get; set; }

        [Required]
        public int ProductID { get; set; }

        public virtual Products Products { get; set; }

        [Required]
        [Display(Name ="Số lượng")]
        public int Quantity { get; set; }


        [Display(Name = "Giá")]
        [Range(1, 100), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Price { get; set; }


    }
}
