
using Microsoft.AspNetCore.Mvc;

namespace PrototipoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly IImageService _service;
    public ImageController(IImageService service) //todo aggiungere logger....
    {
        _service = service;
    }


    //creazione del primo endpoint
    [HttpPost("upload/report/{reportId}")] //la chiamiamo cosi perche non è una post normale. salvataggio fisico sul disco (del file), perche scrive sul disco e sul db
    public async Task<IActionResult> UploadFile(IFormFile file, [FromRoute] int reportId ) {

        string result = await _service.UploadFileImages(file, reportId);
        if (result == "-1")
        {
            return BadRequest("File non valido perche vuoto...!");
        }
        //todo .... tutte le altre risposte


        return Created("", result);
    }


}