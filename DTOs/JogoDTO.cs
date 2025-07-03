using GamesList.Models;

namespace GamesList.DTOs
{
    public class JogoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Nota { get; set; }
        public ICollection<string> Generos { get; set; } = [];
        public ICollection<string> Imagens { get; set; } = [];

        public JogoDTO(Jogo jogo)
        {
            Id = jogo.Id;
            Nome = jogo.Nome;
            Nota = jogo.Avaliacoes.Any() ? (int)Math.Round(jogo.Avaliacoes.Average(a => a.Nota)) : 0;
            Generos = [.. jogo.Generos.Select(g => g.Nome)];
            Imagens = [.. jogo.Imagens.Select(i => i.Url)];
            
        }

    }
}