using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(p => p.SortDescription)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Sku)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.MainImageBaseName)
                .HasMaxLength(500);

            builder.Property(p => p.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(p => p.Rating)
                .HasDefaultValue(0);

            builder.Property(p => p.TotalVotes)
                .HasDefaultValue(0);

            builder.Property(p => p.StockQuantity)
                .HasDefaultValue(0);

            builder.Property(p => p.IsActive)
                .HasDefaultValue(true);

            builder.Property(p => p.Composition)
                .HasMaxLength(2000);

            builder.Property(p => p.Allergens)
                .HasMaxLength(1000);

            builder.Property(p => p.WeightInGrams)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(p => p.IsSugarFree)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(p => p.IsGlutenFree)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}