using System.ComponentModel.DataAnnotations;

public class User
{
    public Guid Id { get; set; }

    [Required, MaxLength(100)]
    public string? Name { get; set; }

    [Required, EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? PasswordHash { get; set; } // Store hashed password

    [Required]
    public string? Role { get; set; } // "Admin", "Researcher"
}
