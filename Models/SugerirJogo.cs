namespace GamesList.Models
{
    public class SugerirJogo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public required string Nome { get; set; }
        public ICollection<Genero> Generos { get; set; } = [];
        public ICollection<ImagensSugestao> Imagens { get; set; } = [];
        public required DateTime DataSugestao { get; set; }
        public bool Aprovado { get; set; }
    }
}