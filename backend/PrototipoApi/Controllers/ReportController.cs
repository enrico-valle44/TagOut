using Microsoft.AspNetCore.Mvc;
using PrototipoService.DTO;
using PrototipoService.Services.Interface;

namespace PrototipoApi.Controllers;

[ApiController]
[Route("[controller]")]

public class ReportController : ControllerBase
{
    private readonly IReportService _serviceReport;
    private readonly ILogger<ReportController> _logger;

    public ReportController(IReportService serviceReport, ILogger<ReportController> logger)
    {
        _serviceReport = serviceReport;
        _logger = logger;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllReports()
    {
        _logger.LogInformation("Richiesta GET /report/all");
        var result = await _serviceReport.GetAllReports();
        _logger.LogInformation($"Restituiti {result.Count()} report");
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReportById([FromRoute] int id)
    {
        _logger.LogInformation($"Richiesta GET /report/{id}");

        try
        {
            var result = await _serviceReport.GetReportById(id);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, $"Report con id={id} non trovato");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore imprevisto durante il recupero del report con id={id}");
            return StatusCode(500, "Errore interno del server");
        }
    }

    [HttpPost("add/{idUser}")]
    public async Task<IActionResult> CreateReport([FromRoute] int idUser, [FromBody] ReportDTO reportDTO)
    {
        _logger.LogInformation("Richiesta POST /report/add");

        try
        {
            await _serviceReport.AddReport(idUser,reportDTO);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore imprevisto durante la creazione del report {@Report}", reportDTO);
            return StatusCode(500, "Errore interno del server");
        }
    }

    [HttpPatch("update/{id}")]
    public async Task<IActionResult> UpdateReport([FromRoute] int id, [FromBody] ReportUpdateDTO reportDTO)
    {
        _logger.LogInformation($"Richiesta PATCH /report/update/{id}");

        try
        {
            await _serviceReport.UpdateReport(id, reportDTO);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, $"Tentativo di aggiornare report inesistente con id={id}");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore imprevisto durante l'aggiornamento del report con id={id}");
            return StatusCode(500, "Errore interno del server");
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteReport([FromRoute] int id)
    {
        _logger.LogInformation($"Richiesta DELETE /report/delete/{id}");
        try
        {
            await _serviceReport.DeleteReport(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, $"Tentativo di eliminare report inesistente con id={id}");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore imprevisto durante l'eliminazione del report con id={id}");
            return StatusCode(500, "Errore interno del server");
        }
    }
}
