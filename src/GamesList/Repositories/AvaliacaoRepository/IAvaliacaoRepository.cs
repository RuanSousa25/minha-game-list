using GamesList.Models;

namespace GamesList.Repositories.AvaliacaoRepository
{
    public interface IAvaliacaoRepository
    {
        public Task<Avaliacao?> GetAvaliacaoByIdAsync(int id);
        public Task<List<Avaliacao>> GetAvaliacoesByUsuarioIdAsync(int id);
        public Task<List<Avaliacao>> GetAvaliacoesByJogoIdAsync(int id);
        public void RemoveAvaliacao(Avaliacao avaliacao);
        public void RemoveAvaliacoes(List<Avaliacao> avaliacoes);
        public Task<Avaliacao?> GetAvaliacaoByUsuarioIdAndJogoIdAsync(int usuarioId, int jogoId);
        public Task AddAvaliacaoAsync(Avaliacao avaliacao);
        public void UpdateAvaliacao(Avaliacao avaliacao);

    }
}