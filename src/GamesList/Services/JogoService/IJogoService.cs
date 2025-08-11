using GamesList.Common.Pagination;
using GamesList.Dtos;
using GamesList.Dtos.Responses;
using GamesList.Models;

namespace GamesList.Services.JogoService
{
    public interface IJogoService
    {
        public Task<ServiceResultDto<PagedResult<JogoDto>>> ListJogosAsync(PaginationParams paginationParams);
        public Task<ServiceResultDto<MessageResponseDto>> RemoveJogoAsync(int id);
        public Task<ServiceResultDto<Jogo>> AddJogoAsync(Jogo jogo);
        public Task<ServiceResultDto<bool>> CheckIfJogoExistsAsync(int id);
        public Task<ServiceResultDto<JogoDto>> GetJogoDtoAsync(int id);
        public Task<ServiceResultDto<Jogo>> GetJogoAsync(int id);
    }
}