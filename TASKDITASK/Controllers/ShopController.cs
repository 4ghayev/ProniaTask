using Microsoft.AspNetCore.Mvc;
using TASKDITASK.Contexts;
using TASKDITASK.ViewModels;

public class ShopController : Controller
{
    private readonly AppDbContext _context;

    public ShopController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var products = _context.Products.Select(p => new ShopProductVM
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Rating = p.Rating,
            ImagePath = p.ImagePath
        }).ToList();

        return View(products);
    }
}
