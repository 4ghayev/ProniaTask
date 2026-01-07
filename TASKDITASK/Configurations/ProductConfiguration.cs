using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TASKDITASK.Models;

public partial class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);

        builder.Property(x => x.Description).IsRequired(false).HasMaxLength(256);

        builder.Property(x => x.Price).HasPrecision(10, 2).IsRequired();

        builder.HasCheckConstraint("CK_Product_Price", "[Price]>0");

        builder.Property(x => x.CategoryId).IsRequired();


        // CategoryId int foreign key references Category(Id)
        // Restrict, CASCADE, No action, Set NULL
        builder.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId).HasPrincipalKey(x=>x.Id).OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.ProductTags).WithOne(x => x.Product).HasForeignKey(x => x.ProductId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Cascade);

    }

}
