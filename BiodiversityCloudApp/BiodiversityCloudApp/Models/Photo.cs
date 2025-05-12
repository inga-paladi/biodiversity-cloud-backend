using BiodiversityCloudApp.Common;

namespace BiodiversityCloudApp.Models;

public class Photo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RecordId { get; set; }
    public required string FileType { get; set; }
    public string Path { get; set; } = AppPaths.PhotoUploadFolder;
    public string? Description { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public required ObservationRecord Record { get; set; }
}
