namespace GamesList.Models
{
    public class Genero
    {
        public int Id { get; set; }
        public required string Nome { get; set;}
        public required ICollection<Jogo> Jogo { get; set; }
    }
}