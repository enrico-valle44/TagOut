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
            DateReport = r.DateReport,
            Longitude = r.Longitude,
            Latitude = r.Latitude,

        };
    }
    public async Task<List<ReportViewModel>> GetAllReports()
    {
        return await _context.Reports
                .Select(c => new ReportViewModel
                {
                    Id = c.Id,
                    IdUser = c.IdUser,
                    Title = c.Title,
                    Description = c.Description,
                    DateReport = c.DateReport,
                    Longitude = c.Longitude,
                    Latitude = c.Latitude,

                })
                .ToListAsync();
    }
    public async Task AddReport(ReportDTO reportDTO)
    {
        var user = await _context.UserInfo.FindAsync(reportDTO.IdUser);
        if (user == null)
            throw new KeyNotFoundException($"Utente con id {reportDTO.IdUser} non trovato");
        var report = new Report
        {
            Title = reportDTO.Title,
            Description = reportDTO.Description,
            IdUser = reportDTO.IdUser,
            DateReport = reportDTO.Date,
            Longitude = reportDTO.Lng,
            Latitude = reportDTO.Lat
        };

        await _context.Reports.AddAsync(report);
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

        if (reportDTO.Date.HasValue)
            report.DateReport = reportDTO.Date.Value;

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
            throw new KeyNotFoundException($"Report con id {id} non trovato");
        }

        _context.Reports.Remove(r);
        await _context.SaveChangesAsync();
    }
}
