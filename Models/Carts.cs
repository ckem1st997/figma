﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace figma.Models
{
    public class Carts
    {
        [Index]
        public int RecordID { get; set; }

        [Required]
        public string CartID { get; set; }


        [Required(ErrorMessage = "Chưa nhập thông tin")]
        public int ProductID { get; set; }


        [Display(Name = "Giá")]
        [Range(1, 100), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Price { get; set; }


        [Display(Name = "Số lượng")]
        public int Count { get; set; }


        public string Size { get; set; }
        public string Color { get; set; }

        [Display(Name = "Ngày tạo")]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        public virtual Products Products { get; set; }

    }
}
