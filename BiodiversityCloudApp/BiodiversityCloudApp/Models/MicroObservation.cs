using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BiodiversityCloudApp.Models;

public class MicroObservation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Comment { get; set; }
    public string? PhotoUrl { get; set; }

    public Guid ObservationId { get; set; }
    public Observation Observation { get; set; }
    public Guid AnimalId { get; set; }
    public Animal Animal { get; set; }
}
