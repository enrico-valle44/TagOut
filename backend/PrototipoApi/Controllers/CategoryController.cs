using Microsoft.AspNetCore.Mvc;
using PrototipoService.Model;
using PrototipoService.Services.Interface;

namespace PrototipoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _serviceCategory;
    private readonly ILogger<CategoryController> _logger;
    public CategoryController(ICategoryService serviceCategory, ILogger<CategoryController> logger)
    {
        _serviceCategory = serviceCategory;
        _logger = logger;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Richiesta GET /category/all");
        var result = await _serviceCategory.GetAllCategories();
        _logger.LogInformation($"Restituite {result.Count()} categorie");
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        _logger.LogInformation($"Richiesta GET /category/{id}");
        var result = await _serviceCategory.GetCategoryById(id);
        if (result == null)
        {
            _logger.LogWarning($"Categoria con id={id} non trovata");
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO categoryDTO)
    {
        _logger.LogInformation("Richiesta POST /category/add");

        try
        {
            await _serviceCategory.AddCategory(categoryDTO);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore imprevisto durante la creazione della categoria {@Category}", categoryDTO);
            return StatusCode(500, "Errore interno del server");
        }
    }

    [HttpPatch("update/{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDTO categoryDTO)
    {
        _logger.LogInformation($"Richiesta PATCH /category/update/{id}");
        try
        {
            await _serviceCategory.UpdateCategory(id, categoryDTO);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, $"Tentativo di aggiornare categoria inesistente con id={id}");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore imprevisto durante l'aggiornamento della categoria con id={id}");
            return StatusCode(500, "Errore interno del server");
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] int id)
    {
        _logger.LogInformation($"Richiesta DELETE /category/delete/{id}");
        try
        {
            await _serviceCategory.DeleteCategory(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, $"Tentativo di eliminare categoria inesistente con id={id}");
            return NotFound();
        }
    }
}
