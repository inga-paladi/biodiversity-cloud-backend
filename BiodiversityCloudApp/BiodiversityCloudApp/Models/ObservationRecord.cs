namespace BiodiversityCloudApp.Models;

public class ObservationRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ObservationId { get; set; }
    public Guid AnimalId { get; set; }
    public Location Location { get; set; } = new Location();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Comment { get; set; }

    public required Observation Observation { get; set; }
    public ICollection<Photo> Photos { get; set; } = [];
}
