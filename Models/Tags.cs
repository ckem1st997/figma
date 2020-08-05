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


        [Required]
        public string Name { get; set; }


        [Required]
        public int Soft { get; set; }


        [Required]
        public bool Active { get; set; }

        public virtual List<TagProducts> TagProducts { get; set; }
    }
}
