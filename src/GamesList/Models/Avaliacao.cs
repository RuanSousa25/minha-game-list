namespace GamesList.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }
        public required Usuario Usuario { get; set; }
        public required Jogo Jogo { get; set; }
        public int Nota { get; set; }
        public required string Opiniao { get; set; }
        public DateTime Data { get; set; }
        
    }
}