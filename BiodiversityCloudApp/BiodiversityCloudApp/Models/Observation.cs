using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Observation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Description { get; set; }
    public string Species { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string Notes { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
