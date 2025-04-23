using System.ComponentModel.DataAnnotations.Schema;
using BiodiversityCloudApp.Models.Enums;

namespace BiodiversityCloudApp.Models;

public class Observation()
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = "New Observation";
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; } // nullable to allow for ongoing observations
    public DateTime? UpdatedAt { get; set; }
    public ObservationStatus ObservationStatus { get; set; } = ObservationStatus.New;
    public Location? StartLocation { get; set; }
    public Location? EndLocation { get; set; } // nullable to allow for ongoing observations

    public Guid UserId { get; set; }

    public ResearchType? ResearchType { get; set; }
    public PhenologicalPhase? PhenologicalPhase { get; set; }
    public EnvironmentalConditions? EnvironmentalConditions { get; set; }

    public ICollection<ObservationRecord> Records { get; set; } = [];
    [NotMapped]
    public IEnumerable<Guid> RecordIds {get; set;} = [];
}
