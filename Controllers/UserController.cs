using Microsoft.AspNetCore.Mvc;

namespace Project_for_Aceleration_Csharp_Tryitter.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
