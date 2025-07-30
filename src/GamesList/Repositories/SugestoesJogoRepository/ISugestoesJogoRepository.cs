using GamesList.Models;

namespace GamesList.Repositories.SugestoesJogoRepository
{
    public interface ISugestoesJogoRepository
    {
        public Task<List<SugestaoJogo>> ListSugestoesJogosAsync();
        public Task<SugestaoJogo?> GetSugestaoJogoComRelacoesByIdAsync(int id);
        public Task<SugestaoJogo?> GetSugestaoJogoByIdAsync(int id);
        public Task<List<SugestaoJogo>> GetSugestoesJogoByUsuarioIdAsync(int id);
        public void RemoveSugestao(SugestaoJogo sugerirJogo);
        public Task AddSugestaoJogoAsync(SugestaoJogo sugerirJogo);
    }
}