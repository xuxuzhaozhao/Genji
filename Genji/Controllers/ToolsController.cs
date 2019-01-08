using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Genji.Controllers
{
    public class ToolsController : Microsoft.AspNetCore.Mvc.Controller
    {
        public IActionResult JsonValidate()
        {
            return View();
        }
    }
}