using GamesList.Models;

namespace GamesList.Dtos.Responses
{
    public class AvaliacaoDto(Avaliacao avaliacao)
    {
        public int Id { get; set; } = avaliacao.Id;
        public int? UsuarioId { get; set; } = avaliacao.Usuario?.Id;
        public string? UsuarioLogin { get; set; } = avaliacao.Usuario?.Login;
        public int? JogoId { get; set; } = avaliacao.Jogo?.Id;
        public string JogoNome { get; set; } = avaliacao.Jogo?.Nome ?? string.Empty;
        public int Nota { get; set; } = avaliacao.Nota;
        public string Opiniao { get; set; } = avaliacao.Opiniao;
        public DateTime Data { get; set; } = avaliacao.DataCriacao;

    }

}