using Microsoft.EntityFrameworkCore;
using PrototipoService.Entities;
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

    public async Task<List<CategoryViewModel>> GetAllCategories()
    {
        return await _context.Categories
                     .Select(c => new CategoryViewModel
                     {
                         Id = c.Id,
                         Name = c.Name,
                         Description = c.Description
                     })
                     .ToListAsync();
    }

    public async Task<CategoryViewModel> GetCategoryById(int id)
    {
        var c = await _context.Categories.FindAsync(id);
        if (c == null) return null;

        return new CategoryViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        };
    }
    public async Task AddCategory(CategoryDTO categoryDTO)
    {
        var category = new Category
        {
            Name = categoryDTO.Name,
            Description = categoryDTO.Description
        };

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }
    public Task UpdateCategory(int id)
    {
        throw new NotImplementedException();
    }
    public async Task DeleteCategory(int id)
    {
        var c = await _context.Categories.FindAsync(id);

        if (c == null)
        {
            throw new KeyNotFoundException($"Categoria con id {id} non trovata");
        }

        _context.Categories.Remove(c);
        await _context.SaveChangesAsync();
    }

}
