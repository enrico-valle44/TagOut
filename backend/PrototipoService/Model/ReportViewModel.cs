
using PrototipoService.Entities;

namespace PrototipoService.Model;

public class ReportViewModel
{
    public int Id { get; set; }
    public int IdUser { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime DateReport { get; set; }

    public List<string> Categories { get; set; }
    public List<string> Images { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }

    ////per la navigazione
    //public virtual UserInfo User { get; set; } = null!;

    //public virtual ICollection<Image> Images { get; set; }
    


}
