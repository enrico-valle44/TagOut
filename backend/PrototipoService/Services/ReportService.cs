using Microsoft.EntityFrameworkCore;
using PrototipoService.DTO;
using PrototipoService.Entities;
using PrototipoService.Model;
using PrototipoService.Services.Interface;

namespace PrototipoService.Services;

public class ReportService : IReportService
{
    private readonly DatabaseContext _context;

    public ReportService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<ReportViewModel> GetReportById(int id)
    {
        var r = await _context.Reports.FindAsync(id);
        if (r == null)
            throw new KeyNotFoundException($"Report con id {id} non trovato");

        return new ReportViewModel
        {
            Id = r.Id,
            IdUser = r.IdUser,
            Title = r.Title,
            Description = r.Description,
             DateReport = r.DateReport.ToString("o"), 
             Categories = r.Categories.Select(c => c.Name).ToList(),
            Images = r.Images.Select(i => i.Path).ToList(),
            Longitude = r.Longitude,
            Latitude = r.Latitude,

        };
    }
    public async Task<List<ReportViewModel>> GetAllReports()
    {
        return await _context.Reports
            .Select(r => new ReportViewModel
            {
                Id = r.Id,
                IdUser = r.IdUser,
                Title = r.Title,
                Description = r.Description,
                DateReport = r.DateReport.ToString("o"),             
               Categories = r.Categories.Select(c => c.Name).ToList(),
                Images = r.Images.Select(i => i.Path).ToList(),
                Longitude = r.Longitude,
                Latitude = r.Latitude,

            })
            .ToListAsync();
    }
    public async Task<List<ReportViewModel>> GetAllReportsByUserId(int userId)
    {
        var reports = await _context.Reports
            .AsNoTracking()
            .Where(r => r.IdUser == userId)
            .Include(r => r.User)         
            .Include(r => r.Images)      
            .Include(r => r.Categories)   
            .OrderByDescending(r => r.DateReport)
            .Select(r => new ReportViewModel
            {
                Id = r.Id,
                IdUser = r.IdUser, //teniamolo se no lo restituisce sempre = 0 ... poi vediamo
                Title = r.Title,
                Description = r.Description,
                 DateReport = r.DateReport.ToString("o"), 
                 Categories = r.Categories.Select(c => c.Name).ToList(),
                Images= r.Images.Select(i => i.Path).ToList(),
                Longitude = r.Longitude,
                Latitude = r.Latitude,
            })
            .ToListAsync();

        return reports;
    }

    public async Task AddReport(int idUser, ReportDTO reportDTO)
    {
        var user = await _context.UserInfo.FindAsync(idUser);
        Console.WriteLine(idUser);
        if (user == null)
            throw new KeyNotFoundException($"Utente con idUser {idUser} non trovato");

        var categories = await _context.Categories
         .Where(c => reportDTO.Categories.Contains(c.Name))
         .ToListAsync();

        var report = new Report
        {
            Title = reportDTO.Title,
            Description = reportDTO.Description,
            IdUser = idUser,
            DateReport = DateTime.UtcNow,
            Longitude = reportDTO.Lng,
            Latitude = reportDTO.Lat,
            Categories = categories,
            Images = reportDTO.Images?.Select(img => new Image
            {
                Path = img
            }).ToList() ?? new List<Image>()
        };

        _context.Reports.Add(report);
        await _context.SaveChangesAsync();

       
    }
    public async Task UpdateReport(int id, ReportUpdateDTO reportDTO)
    {
        var report = await _context.Reports
        .Include(r => r.Categories)
        .Include(r => r.Images)
        .FirstOrDefaultAsync(r => r.Id == id);

        if (report == null)
            throw new KeyNotFoundException($"Report con id {id} non trovato");

        if (!string.IsNullOrWhiteSpace(reportDTO.Title))
            report.Title = reportDTO.Title;

        if (!string.IsNullOrWhiteSpace(reportDTO.Description))
            report.Description = reportDTO.Description;

        if (reportDTO.DateReport.HasValue)
            report.DateReport = reportDTO.DateReport.Value;

        if (reportDTO.Lat.HasValue)
            report.Latitude = reportDTO.Lat.Value;

        if (reportDTO.Lng.HasValue)
            report.Longitude = reportDTO.Lng.Value;

        if (reportDTO.Categories != null && reportDTO.Categories.Any())
        {
            var categories = await _context.Categories
                .Where(c => reportDTO.Categories.Contains(c.Name))
                .ToListAsync();

            report.Categories.Clear();
            foreach (var category in categories)
                report.Categories.Add(category);
        }

        if (reportDTO.Images != null)
        {
            _context.Images.RemoveRange(report.Images);

            report.Images = reportDTO.Images
                .Select(url => new Image
                {
                    Path = url,
                    ReportId = report.Id
                })
                .ToList();
        }

        await _context.SaveChangesAsync();
    }
    public async Task DeleteReport(int id)
    {
        var r = await _context.Reports.FindAsync(id);

        if (r == null)
        {
            throw new KeyNotFoundException($"Report con idUser {id} non trovato");
        }

        _context.Reports.Remove(r);
        await _context.SaveChangesAsync();
    }
}
