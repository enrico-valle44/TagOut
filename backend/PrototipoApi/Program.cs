using Microsoft.EntityFrameworkCore;
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

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatabaseContext>(options =>
    {
        //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); //serve per specificare che non vogliamo un contesto in cui vengano segnate le modifiche
        options.UseNpgsql(connectionString); //visto che vogliamo implementare l'ORM con EF... stiamo usando un approccio DbFirst... quando useremo le migrazioni ci svincoleremo da questa robaccia
                                             //bel commento ;-)
    }
    );

//abbiamo bisogno dei service, che viene distrutto alla fine di ogni utilizzo
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserInfoService, UserInfoService>(); 
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IReportService, ReportService>();

//test brutale di connessione
try
{
    using var conn = new NpgsqlConnection(connectionString);
    conn.Open();
    Console.WriteLine("Connessione ok");
}
catch (Exception ex)
{
    Console.WriteLine("Errore connessione: " + ex.Message);
       
}

//buildiamo l'app
var app = builder.Build();


// Configure the HTTP request pipeline. usiamo swagger solo se siamo in modalita sviluppo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers(); //va a mappare 

app.Run();
