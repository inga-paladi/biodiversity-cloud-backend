using System.ComponentModel.DataAnnotations.Schema;

public class Comment
{
    public Guid Id { get; set; }
    public string? Text { get; set; }

    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User? User { get; set; }

    [ForeignKey("Observation")]
    public Guid ObservationId { get; set; }
    public Observation? Observation { get; set; }
}
