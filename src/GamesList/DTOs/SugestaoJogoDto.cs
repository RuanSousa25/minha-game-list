using GamesList.Models;

namespace GamesList.Dtos
{
    public class SugestaoJogoDto(SugestaoJogo sugestao)
    {
        public int Id { get; set; } = sugestao.Id;
        public int? JogoAprovadoId { get; set; } = sugestao.JogoAprovadoId;
        public int UsuarioId { get; set; } = sugestao.UsuarioId;
        public string Nome { get; set; } = sugestao.Nome;
        public ICollection<string> Generos { get; set; } = [.. sugestao.Generos.Select(g => g.Nome)];
        public string? ImagemCapa { get; set; } = sugestao.Imagens.FirstOrDefault(i => i.TipoId == 1)?.Url;
        public string? ImagemIcone { get; set; } = sugestao.Imagens.FirstOrDefault(i => i.TipoId == 2)?.Url;
        public DateTime DataSugestao { get; set; } = sugestao.DataSugestao;
        public bool Aprovado { get; set; } = sugestao.Aprovado;
    }
}