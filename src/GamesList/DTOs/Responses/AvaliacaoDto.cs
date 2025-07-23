using GamesList.Models;

namespace GamesList.Dtos.Responses
{
    public class AvaliacaoResponseDto
    {
        public int Id { get; set; }
        public int? UsuarioId { get; set; }
        public string? UsuarioLogin { get; set; }
        public int? JogoId { get; set; }
        public string JogoNome { get; set; } = string.Empty;
        public int Nota { get; set; }
        public string Opiniao { get; set; } = string.Empty;
        public DateTime Data { get; set; }

        public static AvaliacaoResponseDto FromEntity(Avaliacao avaliacao)
        {
            return new AvaliacaoResponseDto
            {
                Id = avaliacao.Id,
                UsuarioId = avaliacao.Usuario?.Id,
                UsuarioLogin = avaliacao.Usuario?.Login,
                JogoId = avaliacao.Jogo?.Id,
                JogoNome = avaliacao.Jogo?.Nome ?? string.Empty,
                Nota = avaliacao.Nota,
                Opiniao = avaliacao.Opiniao,
                Data = avaliacao.Data,
            };

        }

    }

}