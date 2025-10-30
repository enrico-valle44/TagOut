using Microsoft.EntityFrameworkCore;
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
        try
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
        catch (Exception ex)
        {
            throw new Exception("Errore durante il recupero di tutte le categorie", ex);
        }
    }

    public async Task<CategoryViewModel> GetCategoryById(int id)
    {
        try
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
        catch (Exception ex)
        {
            throw new Exception($"Errore durante il recupero della categoria con id {id}", ex);
        }
    }
    public async Task AddCategory(CategoryDTO categoryDTO)
    {
        var category = new Category
        {
            Name = categoryDTO.Name,
            Description = categoryDTO.Description
        };

        try
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("Errore durante l'aggiunta della categoria", ex);
        }
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

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException dbEx)
        {
            throw new Exception($"Errore durante l'aggiornamento della categoria con id {id}", dbEx);
        }
    }
    public async Task DeleteCategory(int id)
    {
        var c = await _context.Categories.FindAsync(id);

        if (c == null)
        {
            throw new KeyNotFoundException($"Categoria con id {id} non trovata");
        }

        try
        {
            _context.Categories.Remove(c);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception($"Errore durante l'eliminazione della categoria con id {id}", ex);
        }
    }

}
