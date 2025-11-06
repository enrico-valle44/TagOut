
namespace PrototipoService.Entities;

public class Category
{
    public int Id { get; set; } 

    public required string Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}
