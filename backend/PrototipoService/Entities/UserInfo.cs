
namespace PrototipoService.Entities;

public class UserInfo
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Gender { get; set; }
    public DateTime DOB { get; set; }
    public virtual ICollection<Report> Reports { get; set; }


}
