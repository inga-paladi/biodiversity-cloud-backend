using BiodiversityCloudApp.DTOs.Common;

namespace BiodiversityCloudApp.DTOs.ObservationRecords;

public class UpdateObservationRecordDto
{
    public Guid? AnimalId { get; set; }
    public LocationDto? Location { get; set; } = new LocationDto();
    public DateTime? Timestamp { get; set; } = DateTime.UtcNow;
    public string? Comment { get; set; }
}
