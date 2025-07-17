using GamesList.Databases;
using GamesList.Dtos;
using GamesList.Dtos.Responses;
using GamesList.Models;

namespace GamesList.Services.ImagensService
{
    public interface IImagensService
    {
        public Task<ServiceResultDto<MessageResponseDto>> RemoveImagensByJogoIdAsync(int id);
        public Task<ServiceResultDto<MessageResponseDto>> AddImagemAsync(Imagem imagem);
    }
}