
namespace PrototipoService.DTO;

public class ReportDTO
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int IdUser { get; set; }
    public DateTime Date { get; set; }
    public List<string>? Categories { get; set; }
    public List<string>? Images { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }

}
