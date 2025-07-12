namespace GamesList.DTOs.Requests
{
    public class UploadGameRequest
    {
        public required string Nome { get; set; }
        public required ICollection<int> Generos { get; set; }
    }
}