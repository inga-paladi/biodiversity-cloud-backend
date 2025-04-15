using BiodiversityCloudApp.Models.Enums;

namespace BiodiversityCloudApp.DTOs
{
    public class ObservationDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }          
        public string Description { get; set; }
        public string Species { get; set; }
        public string Location { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string Notes { get; set; }
        public string ObserverName { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public string Weather { get; set; }
        public string AdditionalDetails { get; set; }
        public ResearchType ResearcherType { get; set; }
        public PhenologicalPhase PhenologicalPhase { get; set; }
        public ObservationStatus ObservationStatus { get; set; } = ObservationStatus.New;

        public List<PhotoDto> Photos { get; set; } = [];
        public List<CommentDto> Comments { get; set; } = [];
        public List<MicroObservationDto> MicroObservations { get; set; } = [];
    }
}

