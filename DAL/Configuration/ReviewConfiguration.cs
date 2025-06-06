﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {

            builder
                 .HasKey(r => r.Id);

            builder
                .Property(r => r.Id)
                .HasDefaultValueSql("NEWID()");


            builder.HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.AppUser)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(r => r.ReviewRating)
                .IsRequired();

            builder.Property(r => r.Comment)
                .HasMaxLength(500);

            builder.ToTable(t =>
                t.HasCheckConstraint("CK_Review_Rating", "[ReviewRating] BETWEEN 1 AND 5"));
        }
    }
}
