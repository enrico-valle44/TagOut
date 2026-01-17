using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Npgsql;
using PrototipoApi.Controllers;
using PrototipoService;
using PrototipoService.Services;
using PrototipoService.Services.Interface;

var builder = WebApplication.CreateBuilder(args); //creiamo l app con il builder

builder.Services.AddControllers(); //builder.Services contiene tutte le dipendenze dell'applicativo, usiamo anche quelle di microsoft
// Learn more https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DatabaseContext>(options =>
    {
        //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); //serve per specificare che non vogliamo un contesto in cui vengano segnate le modifiche
        options.UseNpgsql(connectionString); //visto che vogliamo implementare l'ORM con EF... stiamo usando un approccio DbFirst... quando useremo le migrazioni ci svincoleremo da questa robaccia
                                             //bel commento ;-)
    });

//abbiamo bisogno dei service, che viene distrutto alla fine di ogni utilizzo
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserInfoService, UserInfoService>(); 
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IGeoService, GeoService>();
builder.Services.AddControllers()
       .AddNewtonsoftJson(); // necessario per GeoJSON.Net

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:8080", "http://backend:5089", "http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


//buildiamo l'app
var app = builder.Build();

// Serve immagini statiche
var filePath = builder.Configuration["FilePath"] ?? "/app/immagini";
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(filePath),
    RequestPath = "/immagini"
});


// 2. Usa CORS
app.UseCors(MyAllowSpecificOrigins);

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        db.Database.Migrate();

        if (!db.Categories.Any())
        {
            db.Categories.AddRange(
                new PrototipoService.Entities.Category { Name = "Sport", Description = "Graffiti riguardo lo sport, principalmente squadre di calcio" },
                new PrototipoService.Entities.Category { Name = "Politica", Description = "Graffiti riguardo la politica e l'attivismo" },
                new PrototipoService.Entities.Category { Name = "Cultura", Description = "Graffiti riguardo la cultura  e l'arte in generale" }
            );
            db.SaveChanges();
            Console.WriteLine("Categorie iniziali inserite nel database.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migrazione Database fallita: {ex.Message}");
    }
}

app.UseAuthorization();

// Configure the HTTP request pipeline. usiamo swagger solo se siamo in modalità sviluppo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers(); //va a mappare 

app.Run();
