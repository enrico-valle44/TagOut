using Microsoft.AspNetCore.Mvc;
using PrototipoService.DTO;
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
    public async Task<IActionResult> GetAllCategories()
    {
        _logger.LogInformation("Richiesta GET /category/all");
        var result = await _serviceCategory.GetAllCategories();
        _logger.LogInformation($"Restituite {result.Count()} categorie");
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] int id)
    {
        _logger.LogInformation($"Richiesta GET /category/{id}");

        try
        {
            var result = await _serviceCategory.GetCategoryById(id);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, $"Categoria con id={id} non trovata");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore imprevisto durante il recupero della categoria con id={id}");
            return StatusCode(500, "Errore interno del server");
        }
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
    public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] CategoryUpdateDTO categoryDTO)
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
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore imprevisto durante l'eliminazione della categoria con id={id}");
            return StatusCode(500, "Errore interno del server");
        }
    }
}
