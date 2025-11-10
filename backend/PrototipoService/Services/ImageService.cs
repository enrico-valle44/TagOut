using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PrototipoApi.Controllers;

namespace PrototipoService.Services;

public class ImageService : IImageService
{
    private readonly DatabaseContext _context; //per poter popolare il db ecc..
    private readonly IConfiguration _configuration; //interfaccia che mappa tutti i setting dell'applicativo, estensione di microsoft. mappa l'appsetting in chiave-valore
    //costruttore, serve dbcontext e appconfigurations per recuperare i settings dei nostri applicativi
    public ImageService(DatabaseContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string> UploadFileImages(IFormFile file, int reportId) 
    {
        //validazione file 
        if(file.Length < 1)
        {
            return await Task.FromResult("-1"); //fare un return -1 ma si aspetta un Task
        }
        //validazione report, controlliamo la sua esistenza
        //bool exist = await _context.Reports.Where(x => x.Id == reportId).AnyAsync();
        //if (exist == false)
        //{
        //    return await Task.FromResult("-2"); //todo gestire le badrequest
        //}

        //recuperare da appsettings il path su cui salvare il file 
        string? filePath = _configuration["FilePath"]; //filepath restituisce una stringa forse???
        if(string.IsNullOrWhiteSpace(filePath))
        {
            return await Task.FromResult("-3");
        }

        //controllo se la directory esiste 
        if (!Directory.Exists(filePath))
        {
            return await Task.FromResult("-4");
        }

        //nel caso trovi il file utilizziamo classe statica di sistema per fare il combine
        string filePathFull = Path.Combine(filePath, file.FileName); //combine del filepath dell'appsetting con il nome del file (proprieta del formfile che abbiamo in input)

        //todo BEGIN transazione

        //var imageEntity = new Image
        //{
        //    ReportId = reportId,
        //    Path = filePathFull,
        //};

        //await _context.Images.AddAsync(imageEntity);
        //await _context.SaveChangesAsync();

        

        using (Stream fileStream = new FileStream(filePathFull, FileMode.Create)) //usiamo lo stream per caricare il file
        {
            await file.CopyToAsync(fileStream); //prende file e lo copia nello stream che agisce gia direttamente sul disco, con questa operazione si scrive sul disco
        }

          

        //todo COMMIT transazione


        //se fallisce una delle 2 operazioni (begin o commit): ROLLBACK e si annulla la begin
        return filePathFull;
       
    } 
}
