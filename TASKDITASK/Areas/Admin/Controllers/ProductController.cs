using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TASKDITASK.Contexts;
using TASKDITASK.Models;
using TASKDITASK.ViewModels;
using TASKDITASK.Extensions;

namespace TASKDITASK.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductTags)
                    .ThenInclude(pt => pt.Tag)
                .ToListAsync();

            return View(products);
        }

     
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();

            if (!ModelState.IsValid)
                return View(vm);

            bool isExistCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
            if (!isExistCategory)
            {
                ModelState.AddModelError("CategoryId", "Bele bir category movcud deyil");
                return View(vm);
            }

            if (vm.Image == null)
            {
                ModelState.AddModelError("Image", "Sekil secilmeyib");
                return View(vm);
            }

            if (!vm.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Sekil formatinda olmalidir");
                return View(vm);
            }

            if (!vm.Image.IsAllowedSize(2))
            {
                ModelState.AddModelError("Image", "Sekil maksimum 2MB ola biler");
                return View(vm);
            }

            string fileName = Guid.NewGuid() + Path.GetExtension(vm.Image.FileName);
            string path = Path.Combine(_env.WebRootPath, "assets/images/website-images", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await vm.Image.CopyToAsync(stream);
            }

            Product product = new Product
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                CategoryId = vm.CategoryId,
                ImagePath = fileName,
                Rating = vm.Rating,
                ProductTags = vm.TagIds?.Select(id => new ProductTag
                {
                    TagId = id
                }).ToList()
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();

            var product = await _context.Products
                .Include(p => p.ProductTags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            ProductUpdateVM vm = new ProductUpdateVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Rating = product.Rating,
                TagIds = product.ProductTags.Select(pt => pt.TagId).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductUpdateVM vm)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();

            if (!ModelState.IsValid)
                return View(vm);

            var product = await _context.Products
                .Include(p => p.ProductTags)
                .FirstOrDefaultAsync(p => p.Id == vm.Id);

            if (product == null)
                return NotFound();

            product.Name = vm.Name;
            product.Description = vm.Description;
            product.Price = vm.Price;
            product.CategoryId = vm.CategoryId;
            product.Rating = vm.Rating;

            _context.ProductTags.RemoveRange(product.ProductTags);

            product.ProductTags = vm.TagIds?.Select(id => new ProductTag
            {
                TagId = id
            }).ToList();

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductTags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            _context.ProductTags.RemoveRange(product.ProductTags);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
