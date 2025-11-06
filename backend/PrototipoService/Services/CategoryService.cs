using Microsoft.EntityFrameworkCore;
using PrototipoService.DTO;
using PrototipoService.Entities;
using PrototipoService.Model;
using PrototipoService.Services.Interface;

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
        if (c == null)
            throw new KeyNotFoundException($"Categoria con id {id} non trovata");

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
    public async Task UpdateCategory(int id, CategoryUpdateDTO categoryDTO)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            throw new KeyNotFoundException($"Categoria con id {id} non trovata");

        if (!string.IsNullOrWhiteSpace(categoryDTO.Name))
            category.Name = categoryDTO.Name;

        if (!string.IsNullOrWhiteSpace(categoryDTO.Description))
            category.Description = categoryDTO.Description;

        await _context.SaveChangesAsync();

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
