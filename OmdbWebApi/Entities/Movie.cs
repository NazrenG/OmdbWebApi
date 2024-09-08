namespace OmdbWebApi.Entities
{
    public class Movie
    {
        public int Id { get; set; } 
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
