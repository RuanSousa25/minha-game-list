using GamesList.Models;
using GamesList.Models.Enums;

namespace GamesList.Dtos
{
    public class JogoDto(Jogo jogo)
    {
        public int Id { get; set; } = jogo.Id;
        public string Nome { get; set; } = jogo.Nome;
        public int Nota { get; set; } = jogo.Avaliacoes.Count > 0 ? (int)Math.Round(jogo.Avaliacoes.Average(a => a.Nota)) : 0;
        public int AvaliacoesCount { get; set; } = jogo.Avaliacoes.Count;
        public ICollection<string> Generos { get; set; } = [.. jogo.Generos.Select(g => g.Nome)];
        public string? ImagenCapa { get; set; } = jogo.Imagens.SingleOrDefault(i => i.TipoId == (int)TipoImagemEnum.Capa)?.Url;
        public string? ImagenIcon { get; set; } = jogo.Imagens.SingleOrDefault(i => i.TipoId == (int)TipoImagemEnum.Icone)?.Url
        ;
    }
}