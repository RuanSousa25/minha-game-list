using GamesList.Models;

namespace GamesList.Repositories.SugerirJogoRepository
{
    public interface ISugerirJogoRepository
    {
        public Task<List<SugerirJogo>> ListSugerirJogosAsync();
        public Task<SugerirJogo?> GetSugerirJogoComRelacoesByIdAsync(int id);
        public Task<SugerirJogo?> GetSugerirJogoByIdAsync(int id);
        public Task<List<SugerirJogo>> GetSugerirJogoByUsuarioIdAsync(int id);
        public void RemoveSugestao(SugerirJogo sugerirJogo);
        public Task AddSugerirJogoAsync(SugerirJogo sugerirJogo);
    }
}