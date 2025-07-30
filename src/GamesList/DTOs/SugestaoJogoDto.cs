using GamesList.Models;

namespace GamesList.Dtos
{
    public class SugestaoJogoDto(SugestaoJogo sugestao)
    {
        public int Id { get; set; } = sugestao.Id;
        public int UsuarioId { get; set; } = sugestao.UsuarioId;
        public string Nome { get; set; } = sugestao.Nome;
        public ICollection<string> Generos { get; set; } = [.. sugestao.Generos.Select(g => g.Nome)];
        public ICollection<string> Imagens { get; set; } = [.. sugestao.Imagens.Select(i => i.Url)];
        public DateTime DataSugestao { get; set; } = sugestao.DataSugestao;
        public bool Aprovado { get; set; } = sugestao.Aprovado;
    }
}