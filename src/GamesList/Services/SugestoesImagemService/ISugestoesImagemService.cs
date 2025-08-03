using GamesList.Dtos;
using GamesList.Dtos.Responses;
using GamesList.Models;
namespace GamesList.Services.SugestoesImagemService
{
    public interface ISugestoesImagemService
    {
        public Task<ServiceResultDto<MessageResponseDto>> SaveImagemAsync(int sugestaoJogoId, string url, int tipoId);
        public ServiceResultDto<MessageResponseDto> RemoveSugestoesImagem(List<SugestaoImagem> imagens);
        public Task<ServiceResultDto<MessageResponseDto>> AddImagemAsync(SugestaoImagem imagem);
    }
}