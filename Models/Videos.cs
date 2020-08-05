using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class Videos
    {
        [Key]
        [Index]
        public int Id { get; set; }

        [Display(Name="Tên videos")]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Image { get; set; }

        public string VideoLink { get; set; }


        public string Body { get; set; }

        public bool Active { get; set; }


        public bool Home { get; set; }


        public int Soft { get; set; }



        [Display(Name = "Ngày tạo")]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
    }
}
