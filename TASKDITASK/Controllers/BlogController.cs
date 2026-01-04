using Microsoft.AspNetCore.Mvc;

namespace TASKDITASK.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
