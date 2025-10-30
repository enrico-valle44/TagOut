using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrototipoService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoService.Configurations;

public class CategoryEntityTypeConfigurations : IEntityTypeConfiguration<Category>
{

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("category", "cust");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasColumnName("name").IsRequired();
        builder.Property(x => x.Description).HasColumnName("description");
    }
}
