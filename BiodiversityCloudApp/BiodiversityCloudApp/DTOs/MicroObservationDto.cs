using Microsoft.AspNetCore.Mvc;

namespace BiodiversityCloudApp.DTOs
{
    public class MicroObservationDto
    {
        public Guid Id { get; set; }
        public Guid ObservationId { get; set; }
        public Guid AnimalId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Comment { get; set; }
        public string? PhotoUrl { get; set; }
    }

}
