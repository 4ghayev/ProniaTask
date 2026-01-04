using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TASKDITASK.Contexts;
using TASKDITASK.Models;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
        => View(await _context.Categories.ToListAsync());

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateVM vm)
    {
        if (!ModelState.IsValid) return View(vm);

        Category category = new()
        {
            Name = vm.Name
        };

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
