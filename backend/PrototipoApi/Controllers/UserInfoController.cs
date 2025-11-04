using Microsoft.AspNetCore.Mvc;
using PrototipoService.DTO;
using PrototipoService.Model;
using PrototipoService.Services;
using PrototipoService.Services.Interface;
using System.Security.Cryptography.X509Certificates;

namespace PrototipoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserInfoController : ControllerBase
{
    private readonly IUserInfoService _service;
    public UserInfoController(IUserInfoService service) //todo aggiungere logger....
    {
        _service = service;
    }


    [HttpPost] //specifica la route, il verbo post
    public async Task<IActionResult> Add([FromBody] UserInfoDTO addEntity) {


        int result = await _service.AddUserInfo(addEntity);
        if (result == -1)
        {
            return BadRequest("errore: username gia esistente");
        }

        return Created("", result); //status code 201 creared con l'id della risorsa creata
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await _service.GetById(id);
        if (result == null)
        {
            return NotFound("errore: user non trovato");
        }

        return Ok(result); //status code 201 creared con l'id della risorsa creata
    }

    [HttpGet]
    public async Task<IActionResult> GetByFilter([FromQuery] UserInfoFilter filter)
    {
        List<UserInfoViewModel> result = await _service.GetByFilter(filter);
        if (result == null || !result.Any())
        {
            return NoContent();
        }

        return Ok(result); //status code 201 creared con l'id della risorsa creata
    }


    [HttpPatch("{id}")] //aggiorniamo solo dei pezzi dell'oggetto
    public async Task<IActionResult> UpdatePatch([FromRoute] int id, [FromBody] UserInfoUpdateDTO updateEntity)
    {
        int result = await _service.UpdatePatch(updateEntity, id); //l'id lo prende dslla route, l'entity dal body


        if (result == -1)
        {
            return BadRequest("errore: user richiesto da aggiornare non esistente");
        }

        return NoContent(); 
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        int result = await _service.Delete(id); //l'id lo prende dslla route, l'entity dal body

        if (result == -1)
        {
            return BadRequest("errore: user richiesto da aggiornare non esistente");
        }
        return NoContent();
    }



}
