using GamesList.DTOs;
namespace GamesList.Services.ImagensSugestaoService
{
    public interface IImagensSugestaoService
    {
        public Task<ServiceResultDto<string>> SaveImagem(int sugestaoJogoId, string url);
      
    }
}