using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrototipoService.Entities;

namespace PrototipoService.Configurations;

public class ReportEntityTypeConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.HasKey(r => r.Id); 
        builder.Property(r => r.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        builder.Property(r => r.Title).HasColumnName("title");

        builder.Property(r => r.Description).HasColumnName("description");


        builder.Property(r => r.DateReport).HasColumnName("date_report");

        builder.Property(r => r.Longitude).HasColumnName("longitude");

        builder.Property(r => r.Latitude).HasColumnName("latitude");

        builder.Property(r => r.IdUser).HasColumnName("user_id");
        builder
            .HasOne(r => r.User)
            .WithMany(u => u.Reports)
            .HasForeignKey(r => r.IdUser)
            .OnDelete(DeleteBehavior.Cascade);

        builder
           .HasMany(r => r.Categories)
           .WithMany(c => c.Reports)
           .UsingEntity<Dictionary<string, object>>(
               "report_category",
               j => j
                   .HasOne<Category>()
                   .WithMany()
                   .HasForeignKey("category_id")
                   .HasConstraintName("category_id_fk")
                   .OnDelete(DeleteBehavior.Cascade),
               j => j
                   .HasOne<Report>()
                   .WithMany()
                   .HasForeignKey("report_id")
                   .HasConstraintName("report_id_fk")
                   .OnDelete(DeleteBehavior.Cascade),
               j =>
               {
                   j.ToTable("report_category", "cust");
               }
           );


        builder
            .HasMany(r => r.Images)
            .WithOne(i => i.Report)
            .HasForeignKey(i => i.ReportId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("report", "cust"); //nome tabella, nome schema, serve per mapparlo 

    }
}
