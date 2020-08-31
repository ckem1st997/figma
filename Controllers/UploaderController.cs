using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace figma.Controllers
{
    public class UploaderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
