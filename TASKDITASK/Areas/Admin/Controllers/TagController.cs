using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TASKDITASK.Contexts;
using TASKDITASK.Models;
using TASKDITASK.ViewModels.Tag;

namespace TASKDITASK.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
            => View(await _context.Tags.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(TagCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            Tag tag = new()
            {
                Name = vm.Name
            };

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
