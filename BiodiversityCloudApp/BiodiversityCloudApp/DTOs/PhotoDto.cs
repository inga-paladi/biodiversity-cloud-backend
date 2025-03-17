namespace BiodiversityCloudApp.DTOs
{
    public class PhotoDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Url { get; set; }
        public string Description { get; set; }
        public Guid ObservationId { get; set; }
    }
}
