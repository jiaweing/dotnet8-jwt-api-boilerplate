using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Api.Application.Database;

namespace Api.Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _db;

        public HomeController(DatabaseContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Sample home page for anyone including unauthenticated users
        /// </summary>
        [HttpGet("")]
        public IActionResult Index()
        {
            return Ok("Welcome to My Api");
        }

        /// <summary>
        /// Sample dashboard page for authenticated users
        /// </summary>
        [Authorize]
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            return Ok("Welcome to Api Dashboard");
        }
    }
}