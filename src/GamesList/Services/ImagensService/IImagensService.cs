using GamesList.Databases;
using GamesList.DTOs;
using GamesList.Models;

namespace GamesList.Services.ImagensService
{
    public interface IImagensService
    {
        public Task<ServiceResultDto<string>> RemoveImagensByJogoIdAsync(int id);
        public Task<ServiceResultDto<string>> AddImagemAsync(Imagem imagem);
    }
}