namespace PrototipoService.Services.Interface;

public interface IGeoService
{
    public string GetReportsGeoJson();
    public string GetReportsByCategoryGeoJson(string categoryName); 
}
