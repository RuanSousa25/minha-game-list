namespace GamesList.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int JogoId { get; set; }
        public int Nota { get; set; }
        public required string Opiniao { get; set; }
        public DateTime Data { get; set; }
        
    }
}