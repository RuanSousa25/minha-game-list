using GamesList.Common.Pagination;
using GamesList.Dtos;
using GamesList.Dtos.Requests;
using GamesList.Dtos.Responses;
using GamesList.Models;

namespace GamesList.Services.SugestoesJogoService
{
    public interface ISugestoesJogoService
    {
        public Task<ServiceResultDto<int>> SaveSugestaoJogoAsync(UploadGameRequest request, int userId);
        public Task<ServiceResultDto<MessageResponseDto>> SaveSugestaoJogoComImagemAsync(UploadGameRequest request, IFormFile imagemCapa, IFormFile imagemIcone, int userId);
        public Task<ServiceResultDto<JogoDto>> AprovarJogoAsync(int id, int usuarioId);
        public Task<ServiceResultDto<PagedResult<SugestaoJogoDto>>> ListSugerirJogoPagedAsync(PaginationParams paginationParams);
        public Task<ServiceResultDto<PagedResult<SugestaoJogoDto>>> ListSugerirJogoByUserIdPagedAsync(PaginationParams paginationParams, int userId);
        public Task<ServiceResultDto<MessageResponseDto>> RemoverSugestaoJogoAsync(int id);
        public Task<ServiceResultDto<SugestaoJogo>> FindSugestaoJogoAsync(int id);
    }
}