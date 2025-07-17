using GamesList.Dtos;
using GamesList.Dtos.Requests;
using GamesList.Dtos.Responses;
using GamesList.Models;

namespace GamesList.Services.SugerirJogoService
{
    public interface ISugerirJogoService
    {
        public Task<ServiceResultDto<int>> SaveSugestaoJogoAsync(UploadGameRequest request, int userId);
        public Task<ServiceResultDto<MessageResponseDto>> SaveSugestaoJogoComImagemAsync(UploadGameRequest request, IFormFile imagem, int userId);
        public Task<ServiceResultDto<JogoDto>> AprovarJogoAsync(int id);
        public Task<ServiceResultDto<List<SugerirJogoDto>>> ListSugerirJogoAsync();
        public Task<ServiceResultDto<MessageResponseDto>> RemoverSugestaoJogoAsync(int id);
        public Task<ServiceResultDto<SugerirJogo>> FindSugestaoJogoAsync(int id);
    }
}