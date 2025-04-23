using BiodiversityCloudApp.DTOs.Common;
using BiodiversityCloudApp.Models.Enums;

namespace BiodiversityCloudApp.DTOs.Observations;

public class UpdateObservationDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public ObservationStatus? ObservationStatus { get; set; }
    public LocationDto? StartLocation { get; set; }
    public LocationDto? EndLocation { get; set; }
    public ResearchType? ResearchType { get; set; }
    public PhenologicalPhase? PhenologicalPhase { get; set; }
    public EnvironmentalConditionsDto? EnvironmentalConditions { get; set; }
}