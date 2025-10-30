using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoService.Model;
public class UserInfoFilter
{
    public string? Username { get; set; } = null;
    public string? Gender { get; set; } = null;
    public DateTime? DOB { get; set; } = null; 
}
