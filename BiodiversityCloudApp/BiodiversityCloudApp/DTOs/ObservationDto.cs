namespace BiodiversityCloudApp.DTOs
{
    public class ObservationDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }          
        public string Description { get; set; }    
        public DateTime DateObserved { get; set; } 
        public Guid UserId { get; set; }            
        public string ObserverName { get; set; }   

        public List<PhotoDto> Photos { get; set; }     
        public List<CommentDto> Comments { get; set; } 
    }
}

