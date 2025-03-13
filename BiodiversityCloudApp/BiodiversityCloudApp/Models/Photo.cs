using System.ComponentModel.DataAnnotations.Schema;

public class Photo
{
    public Guid Id { get; set; }
    public string? Url { get; set; } // Cloud storage URL

    [ForeignKey("Observation")]
    public Guid ObservationId { get; set; }
    public Observation? Observation { get; set; }
}
