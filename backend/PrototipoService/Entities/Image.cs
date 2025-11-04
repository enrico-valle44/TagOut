using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoService.Entities;

public class Image
{
    public int Id { get; set; }
    public required string Path { get; set; }
    public int ReportId { get; set; } //chiave esterna
    public virtual Report Report{ get; set; } // per navigare dentro images, l'EF con include permette di navigare nelle tabelle (con le join)

}
