using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace figma.Models
{
    public class test
    {
        public int Id { get; set; }

        public List<IFormFile> Photos { get; set; }
    }
}
