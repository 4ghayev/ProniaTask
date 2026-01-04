using Microsoft.AspNetCore.Mvc;

namespace TASKDITASK.Areas.Admin.Controllers;

[Area("Admin")]

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
