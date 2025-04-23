using BiodiversityCloudApp.Models.Enums;
using BiodiversityCloudApp.DTOs.Common;

namespace BiodiversityCloudApp.DTOs.Observations;

public class CreateObservationDto
{
    public string Title { get; set; } = "New Observation";
    public string? Description { get; set; }
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public DateTime? EndTime { get; set; } // nullable to allow for ongoing observations
    public LocationDto? StartLocation { get; set; }
    public LocationDto? EndLocation { get; set; } // nullable to allow for ongoing observations
    public EnvironmentalConditionsDto? EnvironmentalConditions { get; set; }
    public ResearchType? ResearchType { get; set; }
    public PhenologicalPhase? PhenologicalPhase { get; set; }
    public ObservationStatus ObservationStatus { get; set; } = ObservationStatus.New;
}
