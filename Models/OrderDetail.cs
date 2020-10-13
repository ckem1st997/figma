using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class OrderDetail
    {
        [Key, Column(Order = 0)]
        public int OrderId { get; set; }
        [Key, Column(Order = 1)]
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [Key, Column(Order = 2)]
        public string Size { get; set; }

        [Key, Column(Order = 3)]
        public string Color { get; set; }
        public decimal? Price { get; set; }
        public virtual Order Order { get; set; }
        public virtual Products Product { get; set; }
    }
}
