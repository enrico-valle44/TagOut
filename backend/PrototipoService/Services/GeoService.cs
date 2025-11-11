using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PrototipoService.Services.Interface;

namespace PrototipoService.Services;

public class GeoService : IGeoService
{
    private readonly DatabaseContext _context;

    public GeoService(DatabaseContext context)
    {
        _context = context;
    }

    public string GetReportsGeoJson()
    {
        var features = new List<Feature>();

        var reports = _context.Reports
            .Include(r => r.Categories)
            .Include(r => r.Images)
            .ToList();

        foreach (var report in reports)
        {
            var point = new Point(new Position(report.Latitude, report.Longitude));

            var properties = new Dictionary<string, object>
            {
                { "id", report.Id },
                { "description", report.Description },
                { "title", report.Title },
                { "date", report.DateReport.ToString("yyyy-MM-dd") },
                { "categories", report.Categories.Select(c => c.Name).ToList() },
                { "images", report.Images.Select(i => i.Path).ToList() }
            };

            var feature = new Feature(point, properties);
            features.Add(feature);
        }

        var featureCollection = new FeatureCollection(features);

        return JsonConvert.SerializeObject(featureCollection);
    }
}
