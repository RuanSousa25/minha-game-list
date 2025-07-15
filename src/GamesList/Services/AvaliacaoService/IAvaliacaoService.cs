using GamesList.DTOs;
using GamesList.DTOs.Requests;
using GamesList.Models;

namespace GamesList.Services.AvaliacaoService
{
    public interface IAvaliacaoService
    {
        public Task<ServiceResultDto<Avaliacao>> GetAvaliacaoByIdAsync();
        public Task<ServiceResultDto<List<Avaliacao>>> GetAvaliacoesByJogoIdAsync(int id);
        public Task<ServiceResultDto<List<Avaliacao>>> GetAvaliacoesByUsuarioIdAsync(int id);
        public Task<ServiceResultDto<string>> SaveAvaliacaoAsync(int userId, AvaliacaoRequest request);
        public Task<ServiceResultDto<string>> RemoveAvaliacoesByJogoIdAsync(int id);
        public Task<ServiceResultDto<string>> RemoveAvaliacaoByIdAsync(int id, int userId, bool isAdmin);
    }
}