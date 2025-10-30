using Microsoft.EntityFrameworkCore;
using PrototipoService.Model;
using PrototipoService.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoService.Services;

public class CategoryService : ICategoryService
{

    private readonly DatabaseContext _context;
    public CategoryService(DatabaseContext context)
    {
        _context = context;
    }

    public void AddCategory(CategoryDTO categoryDTO)
    {
        throw new NotImplementedException();
    }

    public List<CategoryViewModel> getAllCategories()
    {
        // Query diretta sul DbSet
        var result = _context.Categories
            .OrderBy(c => c.Name)             //ordinae
            .Select(c => new CategoryViewModel //crea un nuovo oggetto viewmodel per ogni record
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            })
            .ToList();                         //restituisce la lista

        return result;
    }

    public CategoryViewModel getCategory(int id)
    {
        throw new NotImplementedException();
    }

    public string GetName()
    {
        var a = _context.Categories.Single(x => x.Id == 1);
        return a.Name ;
    }
}
