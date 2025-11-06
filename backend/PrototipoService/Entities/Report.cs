
namespace PrototipoService.Entities;

public class Report
{
    public int Id { get; set; }
    public int IdUser { get; set; }
    public required string Title{ get; set; }
    public string? Description { get; set; }
    public DateTime DateReport { get; set; }   
    public double Longitude { get; set; }
    public double Latitude { get; set; }

    //per la navigazione
    public virtual UserInfo User { get; set; } = null!;
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    public virtual ICollection<Image> Images { get; set; } = new List<Image>();//un singolo report può avere piu immagini 
}
