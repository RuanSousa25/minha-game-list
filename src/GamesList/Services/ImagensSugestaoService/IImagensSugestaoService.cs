using GamesList.Dtos;
using GamesList.Dtos.Responses;
using GamesList.Models;
namespace GamesList.Services.ImagensSugestaoService
{
    public interface IImagensSugestaoService
    {
        public Task<ServiceResultDto<MessageResponseDto>> SaveImagemAsync(int sugestaoJogoId, string url);
        public ServiceResultDto<MessageResponseDto> RemoveSugestaoImagens(List<ImagensSugestao> imagens);
        public Task<ServiceResultDto<MessageResponseDto>> AddImagemAsync(ImagensSugestao imagem);
    }
}