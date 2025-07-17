using GamesList.Dtos;
using GamesList.Dtos.Requests;
using GamesList.Dtos.Responses;
using GamesList.Models;

namespace GamesList.Services.AvaliacaoService
{
    public interface IAvaliacaoService
    {
        public Task<ServiceResultDto<Avaliacao>> GetAvaliacaoByIdAsync();
        public Task<ServiceResultDto<List<Avaliacao>>> GetAvaliacoesByJogoIdAsync(int id);
        public Task<ServiceResultDto<List<Avaliacao>>> GetAvaliacoesByUsuarioIdAsync(int id);
        public Task<ServiceResultDto<Avaliacao>> SaveAvaliacaoAsync(int userId, AvaliacaoRequest request);
        public Task<ServiceResultDto<MessageResponseDto>> RemoveAvaliacoesByJogoIdAsync(int id);
        public Task<ServiceResultDto<MessageResponseDto>> RemoveAvaliacaoByIdAsync(int id, int userId, bool isAdmin);
    }
}