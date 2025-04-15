using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BiodiversityCloudApp.Models.Enums;

public class Observation()
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Description { get; set; }
    public string Species { get; set; }
    public string Location { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string Notes { get; set; }
    public ObservationStatus ObservationStatus { get; set; } = ObservationStatus.New;

    public Guid UserId { get; set; }
    public User User { get; set; }

    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<MicroObservation> MicroObservations { get; set; } = new List<MicroObservation>();
    public ResearchType ResearcherType { get; set; }
    public PhenologicalPhase PhenologicalPhase { get; set; }
    public EnvironmentalConditions EnvironmentalConditions { get; set; }
}
