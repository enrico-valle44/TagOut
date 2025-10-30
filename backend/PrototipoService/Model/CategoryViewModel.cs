using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoService.Model;

public class CategoryViewModel
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

}
