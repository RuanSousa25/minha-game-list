using GamesList.Models;

namespace GamesList.Dtos
{
    public class JogoDto(Jogo jogo)
    {
        public int Id { get; set; } = jogo.Id;
        public string Nome { get; set; } = jogo.Nome;
        public int Nota { get; set; } = jogo.Avaliacoes.Count > 0 ? (int)Math.Round(jogo.Avaliacoes.Average(a => a.Nota)) : 0;
        public int AvaliacoesCount { get; set; } = jogo.Avaliacoes.Count;
        public ICollection<string> Generos { get; set; } = [.. jogo.Generos.Select(g => g.Nome)];
        public ICollection<string> Imagens { get; set; } = [.. jogo.Imagens.Select(i => i.Url)];
    }
}