using Microsoft.AspNetCore.Mvc;

namespace CRUDpageTest.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Home() 
        { 
            return View();
        }
    }

}
