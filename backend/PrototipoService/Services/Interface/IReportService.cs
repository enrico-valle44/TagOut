
using PrototipoService.DTO;
using PrototipoService.Model;

namespace PrototipoService.Services.Interface;

public interface IReportService
{
    Task<ReportViewModel> GetReportById(int id);
    Task<List<ReportViewModel>> GetAllReports();
    Task<List<ReportViewModel>> GetAllReportsByUserId(int userId);
    Task AddReport(int idUser, ReportDTO reportDTO);
    Task UpdateReport(int id, ReportUpdateDTO reportDTO);
    Task DeleteReport(int id);
}
