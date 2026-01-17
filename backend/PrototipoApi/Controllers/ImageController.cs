
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrototipoService.Services;

namespace PrototipoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageController : ControllerBase
{
    private readonly IImageService _service;
    public ImageController(IImageService service) //todo aggiungere logger....
    {
        _service = service;
    }

    [HttpPost("upload/report/{reportId}")]
    public async Task<IActionResult> UploadFile(IFormFile file, [FromRoute] int reportId)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "File non valido o vuoto." });
        }

        try
        {
            string url = await _service.UploadFileImages(file, reportId);

            if (url == "-1")
                return BadRequest(new { message = "Errore nel salvataggio del file." });

            return Created(url, new { url });
        }
        catch (Exception ex)
        {
            // Log se vuoi aggiungere ILogger
            return StatusCode(500, new { message = "Errore interno durante l'upload.", detail = ex.Message });
        }
    }

    //creazione del primo endpoint
    //[HttpPost("upload/report/{reportId}")] //la chiamiamo cosi perché non è una post normale. salvataggio fisico sul disco (del file), perché scrive sul disco e sul db
    //public async Task<IActionResult> UploadFile(IFormFile file, [FromRoute] int reportId ) {

    //    string result = await _service.UploadFileImages(file, reportId);
    //    if (result == "-1")
    //    {
    //        return BadRequest("File non valido perche vuoto...!");
    //    }
    //    //todo .... tutte le altre risposte


    //    return Created("", result);
    //}

    //[HttpPost("upload")]
    //public async Task<IActionResult> Upload(IFormFile file, int reportId)
    //{
    //    try
    //    {
    //        var path = await _service.UploadImages(file, reportId);
    //        return Ok(new { path });
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(new { message = ex.Message });
    //    }
    //}
}