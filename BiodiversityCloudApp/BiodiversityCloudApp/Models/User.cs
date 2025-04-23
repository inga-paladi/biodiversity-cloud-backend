namespace BiodiversityCloudApp.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; } // Store hashed password
    public string? Role { get; set; } // "Admin", "Researcher"
    public ICollection<Observation> Observations { get; set; } = [];
    public ICollection<Photo> Photos { get; set; } = [];
}
