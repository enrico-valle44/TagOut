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
    List<CategoryViewModel> getAllCategories();
    CategoryViewModel getCategory(int id);
    void AddCategory(CategoryDTO categoryDTO);

}
