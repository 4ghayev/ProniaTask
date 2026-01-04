using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TASKDITASK.Contexts;
using TASKDITASK.Models;

namespace TASKDITASK.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        { 
            var areas=_context.Areas.ToList();
            ViewBag.Areas = areas;
            return View();
        }

      
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
    }
}
