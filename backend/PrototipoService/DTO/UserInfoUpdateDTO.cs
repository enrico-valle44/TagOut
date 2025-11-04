using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoService.Model;

public class UserInfoUpdateDTO
{ 
    //proprieta che vogliamo modificare, l'id no... le altre 3 si ma non è obbligatorio.. quindi mettiamo ? vicino
    public string ? Username { get; set; }
    public string ? Gender { get; set; }
    public DateTime ?DOB { get; set; }
}