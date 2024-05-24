using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Api.Application.Models;

namespace Api.Application.Controllers
{
    public class AdminController : Controller
    {
        /// <summary>
        /// Sample admin dashboard page for users with the admin role
        /// </summary>
        [Authorize(Policy = CustomRoles.Admin)]
        [HttpGet("admin/dashboard")]
        public IActionResult Dashboard()
        {
            return Ok("Welcome Admin");
        }
    }
}
