namespace GamesList.Models
{
    public class Jogo
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public  ICollection<Genero> Generos { get; set; } = [];
        public  ICollection<Avaliacao> Avaliacoes { get; set; } = [];
        public  ICollection<Imagem> Imagens { get; set; } = [];
    }
}