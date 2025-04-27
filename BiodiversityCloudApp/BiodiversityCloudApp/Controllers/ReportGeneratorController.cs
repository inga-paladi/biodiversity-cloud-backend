using BiodiversityCloudApp.Data;
using BiodiversityCloudApp.DTOs;
using Microsoft.AspNetCore.Mvc;

public class ReportGeneratorController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly MicroObservationReportGenerator _reportGenerator;

    public ReportGeneratorController(ApplicationDbContext context, MicroObservationReportGenerator reportGenerator)
    {
        _context = context;
        _reportGenerator = reportGenerator;
    }

    [HttpGet("micro-observations/export")]
    public IActionResult ExportMicroObservationReport()
    {
        var observations = _context.MicroObservations
            .Select(o => new MicroObservationDto
            {
                Id = o.Id,
                Latitude = o.Latitude,
                Longitude = o.Longitude,
                Timestamp = o.Timestamp,
                Comment = o.Comment,
                PhotoUrl = o.PhotoUrl,
                ObservationId = o.ObservationId,
                AnimalId = o.AnimalId
            })
            .ToList();

        var pdfBytes = _reportGenerator.Generate(observations);

        return File(pdfBytes, "application/pdf", "MicroObservationReport.pdf");
    }
}
