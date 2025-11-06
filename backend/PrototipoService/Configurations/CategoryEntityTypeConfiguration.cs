using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrototipoService.Entities;

namespace PrototipoService.Configurations;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasColumnName("name").IsRequired();
        builder.Property(x => x.Description).HasColumnName("description");

        builder
            .HasMany(c => c.Reports)
            .WithMany(r => r.Categories)
            .UsingEntity<Dictionary<string, object>>(
                "report_category",
                j => j
                    .HasOne<Report>()
                    .WithMany()
                    .HasForeignKey("report_id")
                    .HasConstraintName("report_id_fk")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Category>()
                    .WithMany()
                    .HasForeignKey("category_id")
                    .HasConstraintName("category_id_fk")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.ToTable("report_category", "cust");
                }
            );

        builder.ToTable("category", "cust");
    }
}
