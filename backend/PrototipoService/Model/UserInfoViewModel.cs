using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoService.Model;

public class UserInfoViewModel
{
    //si sceglie cosa restituire graficamente, noi scegliamo tutto 
    public int Id { get; set; }
    public string Username { get; set; }
    public string Gender { get; set; }
    public DateTime DOB { get; set; }
}
