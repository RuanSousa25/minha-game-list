using GamesList.Models;

namespace GamesList.DTOs
{
    public class JogoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public ICollection<int> Avaliacoes { get; set; } = [];
        public ICollection<string> Generos { get; set; } = [];
        public ICollection<string> Imagens { get; set; } = [];

        public JogoDTO(Jogo jogo)
        {
            Id = jogo.Id;
            Nome = jogo.Nome;
            Generos = [.. jogo.Generos.Select(g => g.Nome)];
            Avaliacoes = [.. jogo.Avaliacoes.Select(a => a.Id)];
            Imagens = [.. jogo.Imagens.Select(i => i.Url)];
            
        }

    }
}