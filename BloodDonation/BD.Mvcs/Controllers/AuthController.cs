using Microsoft.AspNetCore.Mvc;

namespace BD.Mvcs.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
