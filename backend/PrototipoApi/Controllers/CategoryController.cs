using Microsoft.AspNetCore.Mvc;
using PrototipoService.Model;
using PrototipoService.Services.Interface;

namespace PrototipoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
    {
    private readonly ILogger<CategoryController> _logger;
    private readonly ICategoryService _service;

    public CategoryController(
        ILogger<CategoryController> logger,
        ICategoryService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("all")]
    public IActionResult GetAll()
    {
        // chiama il service
        var categories = _service.getAllCategories();
        // mappo il risultato in una viewmodel,
        var result = categories.Select(c => new CategoryViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        })
        .ToArray();

        return Ok(result);
    }

    [HttpPost]
    public IActionResult Add([FromBody] CategoryDTO categoryDTO)
    {
        _service.AddCategory(categoryDTO);

        _logger.LogInformation("new request category");


        return Created("WeatherForecast/1", 1);
    }

}
