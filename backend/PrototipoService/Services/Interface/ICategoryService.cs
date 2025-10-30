using PrototipoService.Entities;
using PrototipoService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoService.Services.Interface;

public interface ICategoryService
{
    Task<CategoryViewModel> GetCategoryById(int id);
    Task<List<CategoryViewModel>> GetAllCategories();
    Task AddCategory(CategoryDTO categoryDTO);
    Task UpdateCategory(int id, CategoryUpdateDTO categoryDTO);
    Task DeleteCategory(int id);

}
