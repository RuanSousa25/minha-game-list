namespace GamesList.Models
{
    public class SugestaoJogo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int? AprovadorId { get; set;}
        public int? JogoAprovadoId { get; set; }
        public required string Nome { get; set; }
        public ICollection<Genero> Generos { get; set; } = [];
        public ICollection<SugestaoImagem> Imagens { get; set; } = [];
        public bool Aprovado { get; set; }
        public required DateTime DataSugestao { get; set; }
        public DateTime? DataAprovacao { get; set; }
    }
}