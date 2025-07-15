using GamesList.DTOs;
using GamesList.DTOs.Requests;
using GamesList.Models;

namespace GamesList.Services.SugerirJogoService
{
    public interface ISugerirJogoService
    {
        public Task<ServiceResultDto<int>> SaveSugestaoJogoAsync(UploadGameRequest request, int userId);
        public Task<ServiceResultDto<string>> SaveSugestaoJogoComImagemAsync(UploadGameRequest request, IFormFile imagem, int userId);
        public Task<ServiceResultDto<JogoDTO>> AprovarJogoAsync(int id);
        public Task<ServiceResultDto<List<SugerirJogo>>> ListSugerirJogoAsync();
        public Task<ServiceResultDto<string>> RemoverSugestaoJogoAsync(int id);
        public Task<ServiceResultDto<SugerirJogo>> FindSugestaoJogoAsync(int id);
    }
}