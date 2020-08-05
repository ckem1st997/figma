using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class ReviewProduct
    {
        public int Id { get; set; }


        public int ProductId { get; set; }


        public int NumberStart { get; set; }


        [Display(Name ="Tên người đánh giá")]
        public string userID { get; set; }


        public string Content { get; set; }

        public bool StatusReview { get; set; }


        [Display(Name = "Ngày tạo")]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        public virtual Products Products { get; set; }

    }
}
