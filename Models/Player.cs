namespace X00181967CA3.Models
{
    public class Player
    {
        public required string Name { get; set; }
        public required string Team { get; set; }
        public required string Popularity { get; set; }
        public string ImageUrl { get; set; } = "https://via.placeholder.com/50";
    }
}
