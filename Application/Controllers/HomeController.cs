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

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return Ok("Welcome to My Api");
        }

        [HttpGet, Authorize]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            return Ok("Welcome to Api Dashboard");
        }
    }
}