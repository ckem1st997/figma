using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class ConfigSites
    {
        [Key]
        [Index]
        public int ConfigSiteID { get; set; }


        [MaxLength(500)]
        public string Facebook { get; set; }



        [MaxLength(500)]
        public string GooglePlus { get; set; }



        [MaxLength(500)]
        public string Youtube { get; set; }



        [MaxLength(500)]
        public string Linkedin { get; set; }


        [MaxLength(500)]
        public string Twitter { get; set; }


        [MaxLength(4000)]
        public string GoogleAnalytics { get; set; }


        [MaxLength(4000)]
        public string LiveChat { get; set; }


        [MaxLength(4000)]
        public string GoogleMap { get; set; }


        [MaxLength(200)]
        public string Title { get; set; }


        [MaxLength(500)]
        public string Description { get; set; }


        public string ContactInfo { get; set; }


        public string FooterInfo { get; set; }


        [MaxLength(50)]
        public string Hotline { get; set; }


        [MaxLength(100)]
        public string Email { get; set; }


        [MaxLength(500)]
        public string Logo { get; set; }


        public string SaleOffProgram { get; set; }


    }
}
