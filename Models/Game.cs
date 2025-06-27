namespace GamesList.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string? GameName { get; set; }
        public int Rating { get; set; }
        public string? Opinion { get; set; }
        public List<string>? Genres { get; set; } 
    }
}