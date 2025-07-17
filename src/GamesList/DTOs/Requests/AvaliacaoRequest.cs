namespace GamesList.Dtos.Requests
{
    public class AvaliacaoRequest
    {
        public int JogoId { get; set; }
        public int Nota { get; set; }
        public required string Opiniao { get; set; } 
    }
}