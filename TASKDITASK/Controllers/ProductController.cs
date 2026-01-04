using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TASKDITASK.Contexts;

namespace TASKDITASK.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }
    }
}
