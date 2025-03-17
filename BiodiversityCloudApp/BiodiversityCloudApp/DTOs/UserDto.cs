using Microsoft.AspNetCore.Mvc;

namespace BiodiversityCloudApp.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string? Email { get; set; }
    }
}
