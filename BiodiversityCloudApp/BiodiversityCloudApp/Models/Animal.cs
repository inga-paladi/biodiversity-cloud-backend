using System.ComponentModel.DataAnnotations;

namespace BiodiversityCloudApp.Models;

public class Animal
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Name { get; set; }
    public string ScientificName { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; } 

    public string? Category { get; set; }
    public ICollection<Observation> Observations { get; set; } = new List<Observation>();
    public ICollection<ObservationRecord> ObservationRecords { get; set; } = new List<ObservationRecord>();
}
