using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
    public class ProductDetailsConfiguration : IEntityTypeConfiguration<ProductDetails>
    {
        public void Configure(EntityTypeBuilder<ProductDetails> builder)
        {
            builder.HasKey(pd => pd.Id);

            builder.HasOne(pd => pd.Product)
                .WithOne(p => p.ProductDetails)
                .HasForeignKey<ProductDetails>(pd => pd.ProductId);
        }
    }
}
