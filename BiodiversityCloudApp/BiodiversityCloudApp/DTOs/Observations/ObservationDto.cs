using BiodiversityCloudApp.DTOs.Common;
using BiodiversityCloudApp.Models.Enums;

namespace BiodiversityCloudApp.DTOs.Observations;

public class ObservationDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = "New Observation";
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; } // nullable to allow for ongoing observations
    public DateTime? UpdatedAt { get; set; }
    public ObservationStatus ObservationStatus { get; set; } = ObservationStatus.New;
    public LocationDto? StartLocation { get; set; }
    public LocationDto? EndLocation { get; set; }

    public Guid UserId { get; set; } // To be known in case the observation is shared

    public ResearchType ResearchType { get; set; }
    public PhenologicalPhase? PhenologicalPhase { get; set; }
    public EnvironmentalConditionsDto? EnvironmentalConditions { get; set; }

    public List<Guid> RecordIds { get; set; } = [];
}
