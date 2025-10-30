using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrototipoService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoService.Configurations;

public class UserInfoEntityTypeConfiguration : IEntityTypeConfiguration<UserInfo>
{
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.HasKey(u => u.Id); //chiave primaria
        builder.Property(u => u.Id).HasColumnName("id");

        builder.Property(u => u.Username).HasColumnName("username");

        builder.Property(u => u.Gender).HasColumnName("gender");

        builder.Property(u => u.DOB).HasColumnName("dob");

        builder.ToTable("userinfo", "cust"); //nome tabella, nome schema, serve per mapparlo 

    }

}
