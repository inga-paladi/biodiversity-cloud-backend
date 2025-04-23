using BiodiversityCloudApp.DTOs.ObservationRecords;

namespace BiodiversityCloudApp.DTOs
{
    public class AnimalDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ScientificName { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Category { get; set; }
        public ICollection<ObservationRecordDto> ObservationRecords { get; set; }
    }
}
