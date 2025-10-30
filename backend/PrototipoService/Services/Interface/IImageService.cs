using Microsoft.AspNetCore.Http;

namespace PrototipoApi.Controllers;
public interface IImageService
{
    Task<string> UploadFileImages(IFormFile file, int reportId); 

}