using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace User.API.Controllers
{
    
    public class HealthCheckController : Controller
    {
        [HttpGet("healthcheck")]
        [HttpHead("")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}