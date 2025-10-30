using Microsoft.EntityFrameworkCore;
using PrototipoService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoService;
//è l'astrazione del database nel codice 
public class DatabaseContext : DbContext //dbcontext creato da microsoft... pacchetto
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    } //questo perche quando registriamo la dipendezna passiamo delle cose, eredita da base(options) 


    //specificiamo le nostre tabelle
    public DbSet<Category> Categories { get; set; }

    public DbSet<UserInfo> UserInfo { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Report> Reports{ get; set; }

    
    //metodo che gestisce il mapping , sovrascrive il metodo della classe base 
    //tira fuori gli assembly che ci servono per il mapping ( reflection) 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //legge tutte le assembly e configura quelle che servono
    }

}
