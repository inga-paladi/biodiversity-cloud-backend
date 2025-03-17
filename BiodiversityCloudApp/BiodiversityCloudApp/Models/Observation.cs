using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Observation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Species { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string Notes { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}
