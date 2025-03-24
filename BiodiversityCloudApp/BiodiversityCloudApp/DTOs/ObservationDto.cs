namespace BiodiversityCloudApp.DTOs
{
    public class ObservationDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }          
        public string Description { get; set; }
        public string Species { get; set; }
        public string Location { get; set; }
        public DateTime DateObserved { get; set; }
        public string Notes { get; set; }
        public string ObserverName { get; set; }

        public List<PhotoDto> Photos { get; set; } = new List<PhotoDto>();
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }
}

