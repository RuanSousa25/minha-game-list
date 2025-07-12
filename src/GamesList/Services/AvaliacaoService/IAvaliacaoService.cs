using GamesList.DTOs;
using GamesList.DTOs.Requests;
using GamesList.Models;

namespace GamesList.Services.AvaliacaoService
{
    public interface IAvaliacaoService
    {
        public Task<ServiceResultDto<List<Avaliacao>>> GetAvaliacoesByJogoId(int id);
        public Task<ServiceResultDto<List<Avaliacao>>> GetAvaliacoesByUsuarioId(int id);
        public Task<ServiceResultDto<string>> SaveAvaliacao(int userId, AvaliacaoRequest request);
        public Task<ServiceResultDto<string>> RemoveAvaliacoesByJogoId(int id);
    }
}