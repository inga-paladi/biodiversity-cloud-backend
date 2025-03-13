using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Observation
{
    public Guid Id { get; set; }

    [Required]
    public string? Species { get; set; }

    public string? Location { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public string? Notes { get; set; }

    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User? User { get; set; }
}
