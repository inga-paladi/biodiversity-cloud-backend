using BiodiversityCloudApp.DTOs.Common;

namespace BiodiversityCloudApp.DTOs.ObservationRecords;

public class ObservationRecordDto
{
    public Guid Id { get; set; }
    public Guid ObservationId { get; set; }
    public Guid AnimalId { get; set; }
    public LocationDto Location { get; set; } = new LocationDto();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Comment { get; set; }
    public ICollection<Guid> PhotoIds { get; set; } = [];
}
