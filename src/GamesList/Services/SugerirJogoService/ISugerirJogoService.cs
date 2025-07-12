using GamesList.DTOs;
using GamesList.DTOs.Requests;
using GamesList.Models;

namespace GamesList.Services.SugerirJogoService
{
    public interface ISugerirJogoService
    {
        public Task<ServiceResultDto<int>> SaveSugestaoJogo(UploadGameRequest request, int userId);
        public Task<ServiceResultDto<string>> SaveSugestaoJogoComImagem(UploadGameRequest request, IFormFile imagem, int userId);
        public Task<ServiceResultDto<JogoDTO>> AprovarJogo(int id);
        public Task<ServiceResultDto<List<SugerirJogo>>> ListSugerirJogo();
        public Task<ServiceResultDto<string>> RemoverSugestaoJogo(int id);
        public Task<ServiceResultDto<SugerirJogo>> FindSugestaoJogo(int id);
    }
}