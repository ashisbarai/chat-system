using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Web.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        [HttpGet]
        [Route("getprojects")]
        public IActionResult GetProjects()
        {
            var projects = new List<object>{new{Id = 1, Name = "Project 1"}, new { Id = 2, Name = "Project 2" } };
            return Ok(projects);
        }
    }
}
