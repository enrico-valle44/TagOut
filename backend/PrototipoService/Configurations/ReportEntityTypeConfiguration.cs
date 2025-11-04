using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrototipoService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoService.Configurations;

public class ReportEntityTypeConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.HasKey(r => r.Id); 
        builder.Property(r => r.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(r => r.Title).HasColumnName("title");

        builder.Property(r => r.Description).HasColumnName("description");


        builder.Property(r => r.DateReport).HasColumnName("date_report");

        builder.Property(r => r.IdUser).HasColumnName("id_user");
        builder
            .HasOne(r => r.User)
            .WithMany(u => u.Reports)
            .HasForeignKey(r => r.IdUser)
            .OnDelete(DeleteBehavior.Cascade);


        builder.Property(r => r.Longitude).HasColumnName("longitude");

        builder.Property(r => r.Latitude).HasColumnName("latitude");



        builder.ToTable("report", "cust"); //nome tabella, nome schema, serve per mapparlo 

    }
}
