using System.ComponentModel.DataAnnotations;

namespace BiodiversityCloudApp.Models
{
    public class Animal
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }
        public string ScientificName { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; } // optional thumbnail/picture

        // Optional: category (e.g., bird, mammal)
        public string? Category { get; set; }
    }
}
