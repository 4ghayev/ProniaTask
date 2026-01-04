using Microsoft.EntityFrameworkCore;
using TASKDITASK.Models;

namespace TASKDITASK.Contexts;

public class AppDbContext:DbContext
{
    /*    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=LAPTOP-5U2I6SQG\\SQLEXPRESS;Database=ProniaFirstTask;trusted_connection=true;trustservercertificate=true");
            base.OnConfiguring(optionsBuilder);
        }*/

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    
    }

    public DbSet<ShippingArea>Areas { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}
