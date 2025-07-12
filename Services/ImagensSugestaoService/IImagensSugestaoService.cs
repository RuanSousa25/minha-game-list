using GamesList.DTOs;
using GamesList.Models;
namespace GamesList.Services.ImagensSugestaoService
{
    public interface IImagensSugestaoService
    {
        public Task<ServiceResultDto<string>> SaveImagem(int sugestaoJogoId, string url);
        public ServiceResultDto<string> RemoveSugestaoImagens(List<ImagensSugestao> imagens);
        public Task<ServiceResultDto<string>> AddImagemAsync(ImagensSugestao imagem);
    }
}