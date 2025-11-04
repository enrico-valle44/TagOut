using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrototipoService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoService.Configurations;

public class ImageEntityTypeConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(i => i.Id); //chiave primaria
        builder.Property(i => i.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(i => i.Path).HasColumnName("path").IsRequired();

        builder.Property(i => i.ReportId).HasColumnName("report_id");

        builder.ToTable("image", "cust"); //nome tabella, nome schema, serve per mapparlo 
    }
}
