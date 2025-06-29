namespace GamesList.Models
{
    public class Jogo
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required ICollection<Genero> Genero { get; set; }
        public required ICollection<Avaliacao> Avaliacao { get; set; }
        public required ICollection<Imagem> Imagens { get; set;}
    }
}