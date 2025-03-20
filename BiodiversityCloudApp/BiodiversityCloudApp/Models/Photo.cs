using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Photo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Url { get; set; } 
    public string Description { get; set; }
    public Guid ObservationId { get; set; }
    public Observation Observation { get; set; }
}
