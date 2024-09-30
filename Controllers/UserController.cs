using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class UserController:Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}
