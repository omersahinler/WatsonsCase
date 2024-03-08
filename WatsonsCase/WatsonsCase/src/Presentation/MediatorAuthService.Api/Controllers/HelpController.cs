using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WatsonsCase.Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpController : ControllerBase
    {
        [Produces("application/json")]
        [Route("/")]
        [HttpGet]
        public IActionResult Root()
        {
            var deployment = Environment.GetEnvironmentVariable("DEPLOYMENT_INFO");
            return Ok($"V:{deployment} Warmed up sucessfully {DateTime.Now}");
        }
    }
}