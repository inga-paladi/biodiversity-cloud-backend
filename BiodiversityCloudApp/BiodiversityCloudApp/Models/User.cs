using System.ComponentModel.DataAnnotations;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; } // Store hashed password
    public string? Role { get; set; } // "Admin", "Researcher"
    public ICollection<Observation> Observations { get; set; } = new List<Observation>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
