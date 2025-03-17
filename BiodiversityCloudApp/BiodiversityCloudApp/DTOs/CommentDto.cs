namespace BiodiversityCloudApp.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}
