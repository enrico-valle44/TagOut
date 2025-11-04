using PrototipoService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoService.DTO;

public class ReportDTO
{
    public required string Title { get; set; }
    public string ? Description { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public virtual ICollection<Image> Images { get; set; }

}
