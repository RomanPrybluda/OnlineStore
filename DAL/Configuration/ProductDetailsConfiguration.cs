using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
    public class ProductDetailsConfiguration : IEntityTypeConfiguration<ProductDetails>
    {
        public void Configure(EntityTypeBuilder<ProductDetails> builder)
        {

            builder
                .HasKey(pd => pd.Id);

            builder
                .Property(pd => pd.Id)
                .HasDefaultValueSql("NEWID()");

            builder.HasOne(pd => pd.Product)
                .WithOne(p => p.ProductDetails)
                .HasForeignKey<ProductDetails>(pd => pd.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(pd => pd.Sku)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(pd => pd.StockQuantity)
                .IsRequired();
        }
    }
}
