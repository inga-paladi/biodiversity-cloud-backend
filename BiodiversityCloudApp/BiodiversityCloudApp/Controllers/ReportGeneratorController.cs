using BiodiversityCloudApp.DTOs;
using BiodiversityCloudApp.DTOs.Common;
using BiodiversityCloudApp.DTOs.ObservationRecords;
using BiodiversityCloudApp.DTOs.Observations;
using BiodiversityCloudApp.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiodiversityCloudApp.Controllers
{
    public class ReportGeneratorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportGeneratorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("micro-observations/export")]
        public async Task<IActionResult> ExportMicroObservationReport()
        {
            // Get observations with related data
            var observationRecords = await _context.ObservationRecords
                .Include(o => o.Photos)
                .ToListAsync();

            // Get animal data separately
            var animals = await _context.Animals.ToListAsync();
            var animalData = animals.ToDictionary(a => a.Id, a => new AnimalDto
            {
                Id = a.Id,
                Name = a.Name,
                ScientificName = a.ScientificName,
                Description = a.Description,
                ImageUrl = a.ImageUrl,
                Category = a.Category
            });

            // Get observation data separately
            var observations = await _context.Observations.ToListAsync();
            var observationData = observations.ToDictionary(o => o.Id, o => new ObservationDto
            {
                Id = o.Id,
                Title = o.Title,
                Description = o.Description,
                StartTime = o.StartTime,
                EndTime = o.EndTime,
                ResearchType = o.ResearchType ?? ResearchType.Unspecified, // Handle nullable
                PhenologicalPhase = o.PhenologicalPhase
            });

            // Create DTOs
            var observationDtos = observationRecords.Select(o => new ObservationRecordDto
            {
                Id = o.Id,
                ObservationId = o.ObservationId,
                AnimalId = o.AnimalId,
                Location = new LocationDto { Latitude = o.Location.Latitude, Longitude = o.Location.Longitude },
                Timestamp = o.Timestamp,
                Comment = o.Comment,
                PhotoIds = o.Photos.Select(p => p.Id).ToList()
            }).ToList();

            // Generate report
            var document = new MicroObservationReportDocument(observationDtos, animalData, observationData);
            var pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", "Biodiversity_Report.pdf");
        }
    }
}